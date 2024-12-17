using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GamifiedPlatform.Models;
using Microsoft.Data.SqlClient;

namespace GamifiedPlatform.Controllers
{
    public class AdminsController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public AdminsController(GamifiedPlatformContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard(int id)
        {
            var admin = _context.Admins
                .FirstOrDefault(a => a.UserId == id);

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }
        public IActionResult GetAllLeaderboards()
        {


            // Execute the stored procedure to get notifications
            var leaderboards = _context.Leaderboards
                .FromSqlRaw("EXEC getAllLeaderBoards ")
                .ToList();



            return View(leaderboards);
        }
        public IActionResult LeaderboardRank(int leaderboardID)
        {




            // Execute the stored procedure to get notifications
            var boards = _context.Rankings
                .FromSqlRaw($"EXEC LeaderboardRank @LeaderboardID={leaderboardID}")
                .ToList();

            // Pass the learnerId to ViewBag for "Back to Profile" button


            return View(boards);
        }



        public async Task<IActionResult> MarkNotificationAsRead(int notificationID)
        {
            // Ensure notificationID is valid
            if (notificationID <= 0)
            {
                TempData["ErrorMessage"] = "Invalid notification ID.";
                return RedirectToAction("ViewNotifications"); // Redirect to the notifications list or a relevant page
            }

            try
            {
                // Execute the stored procedure
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC markAsRead @notificationID={notificationID}");

                // Optionally, show a success message
                TempData["SuccessMessage"] = "Notification marked as read.";
            }
            catch (Exception ex)
            {
                // Log or handle any errors
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // Redirect to the appropriate view after marking the notification as read
            return RedirectToAction("ViewNotifications"); // Adjust based on your app's structure
        }

        public IActionResult EmotionalFeedbackTrend()
        {
            return View();
        }

        // POST: Fetch Trends Data
        [HttpPost]
        public IActionResult EmotionalFeedbackTrend(int courseID, int moduleID, DateTime timePeriod)
        {
            // Call the stored procedure
            var trends = _context.EmotionalFeedbacks
                .FromSqlRaw("EXEC EmotionalTrendAnalysisIns @CourseID={0}, @ModuleID={1}, @TimePeriod={2}",
                            courseID, moduleID, timePeriod)
                .ToList();

            return View(trends); // Pass trends data to the view
        }
        public IActionResult Profile(int id)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.UserId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            var gamifiedPlatformContext = _context.Admins.Include(a => a.User);
            return View(await gamifiedPlatformContext.ToListAsync());
        }
       
        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }
        public async Task<IActionResult> AddForum(int adminID, int moduleID, int courseID, string title, string description)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(l => l.AdminId == adminID);

            if (admin == null)
            {
                ModelState.AddModelError("", "The specified instructor does not exist.");
                return View("Profile", admin); // Return a new model to prevent null
            }
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec CreateDiscussion @ModuleID={moduleID},@courseID={courseID},@title={title},@description={description}");
            }
            catch (SqlException ex)
            {

                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {

                    var moduleExists = await _context.Modules.AnyAsync(d => d.ModuleId == moduleID);
                    if (!moduleExists)
                    {
                        ModelState.AddModelError("", "The specified module does not exist.");

                    }
                    else ModelState.AddModelError("", "The specified course does not exist.");

                }
                else if (ex.Message.Contains("PRIMARY KEY constraint"))
                {
                    ModelState.AddModelError("", "This forum already Exists");
                }
                else
                {

                    ModelState.AddModelError("", "An error occurred while creating the forum. Please try again.");
                }

                return View("Profile", admin);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("Profile", admin);
            }
            ViewData["SuccessMessage"] = "Forum created successfully!";
            return View("Profile", admin);
        }
        // GET: Admins/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,UserId,Name,Gender,BirthDate,Country,Email")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", admin.UserId);
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", admin.UserId);
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,UserId,Name,Gender,BirthDate,Country,Email")] Admin admin)
        {
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", admin.UserId);
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }
        public IActionResult ViewNotifications(int adminID)
        {
            var adminIdParam = new SqlParameter("@adminID", adminID);

            // Execute the stored procedure to get notifications
            var notifications = _context.Notifications
                .FromSqlRaw("EXEC ViewNot @adminID", adminIdParam)
                .ToList();

            // Pass the learnerId to ViewBag for "Back to Profile" button
            ViewBag.AdminID = adminID;

            return View(notifications);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
