using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GamifiedPlatform.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GamifiedPlatform.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public InstructorsController(GamifiedPlatformContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard(int id)
        {
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.UserId == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        public IActionResult Profile(int id)
        {
            var instructor = _context.Instructors.FirstOrDefault(i => i.UserId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        public IActionResult SendNotification()
        {
            return View();
        }

        // POST: Instructor/SendNotification
        [HttpPost]
        public IActionResult SendNotification(int LearnerID, string Message, string UrgencyLevel)
        {
            try
            {
                // Check if the learner exists (for example, using a database query)
                var learner = _context.Learners.FirstOrDefault(l => l.LearnerId == LearnerID);
                if (learner == null)
                {
                    ViewData["ErrorMessage"] = "The specified learner does not exist.";
                    return View();  // Stay on the same page
                }

                // Generate a new Notification ID (or get it from the database)
                int notificationID = GetNextNotificationID();  // This method should handle ID generation logic

                // Send notification (you might call a stored procedure here)
                var result = _context.Database.ExecuteSqlRaw("EXEC dbo.AssessmentNot @NotificationID, @timestamp, @message, @urgencylevel, @LearnerID",
                    new SqlParameter("@NotificationID", notificationID),
                    new SqlParameter("@timestamp", DateTime.Now),
                    new SqlParameter("@message", Message),
                    new SqlParameter("@urgencylevel", UrgencyLevel),
                    new SqlParameter("@LearnerID", LearnerID));

                if (result > 0)
                {
                    // Set success message and clear form fields
                    ViewData["SuccessMessage"] = "Notification sent successfully!";
                    return View(new Notification());  // Return the empty form after success
                }
                else
                {
                    ViewData["ErrorMessage"] = "An error occurred while sending the notification.";
                    return View();  // Stay on the same page
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
                return View();  // Stay on the same page
            }
        }



        private int GetNextNotificationID()
        {
            var lastNotification = _context.Notifications.OrderByDescending(n => n.Id).FirstOrDefault();
            return lastNotification?.Id + 1 ?? 1;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            var gamifiedPlatformContext = _context.Instructors.Include(i => i.User);
            return View(await gamifiedPlatformContext.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,UserId,Name,LatestQualification,ExpertiseArea,Email")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", instructor.UserId);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", instructor.UserId);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorId,UserId,Name,LatestQualification,ExpertiseArea,Email")] Instructor instructor)
        {
            if (id != instructor.InstructorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.InstructorId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", instructor.UserId);
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorId == id);
        }
    }
}
