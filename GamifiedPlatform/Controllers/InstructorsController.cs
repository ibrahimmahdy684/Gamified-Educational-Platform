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

        // Action to fi
        public async Task<IActionResult> UpdateInfo(int instructorID,string name,string latest_qualification, string expertise_area, string email)
            
        {
            var learnerExists = await _context.Instructors.AnyAsync(d => d.InstructorId == instructorID);
            if (!learnerExists)
            {
                ModelState.AddModelError("", "The specified learner does not exist.");
                return View("UpdateInfo");
            }
            else
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec updateInstructorInfo @InstructorID={instructorID},@Name={name},@LatestQualification={latest_qualification},@ExpertiseArea={expertise_area},@Email={email}");
                TempData["SuccessMessage"] = "Instructor information updated successfully!";
                return RedirectToAction("Index");
            }
            
        }
        public async Task<IActionResult> AddForum(int instructorID,int moduleID, int courseID, string title, string description)
        {
            var instructor = await _context.Instructors.FirstOrDefaultAsync(l => l.InstructorId ==instructorID );

            if (instructor == null)
            {
                ModelState.AddModelError("", "The specified instructor does not exist.");
                return View("Profile", instructor); // Return a new model to prevent null
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

                return View("Profile",instructor);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("Profile",instructor);
            }
            ViewData["SuccessMessage"] = "Forum created successfully!";
            return View("Profile",instructor);
        }
        [HttpPost]
        [HttpPost]
        
        public async Task<IActionResult> AddPath(int instructorID,int LearnerID, int profileID, string completionStatus, string customContent, string adaptiveRules)
        {

            var instructor = await _context.Instructors.FirstOrDefaultAsync(l => l.InstructorId == instructorID);

            if (instructor == null)
            {
                
                return RedirectToAction("Profile"); // Redirect to the Profile action if learner does not exist
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec NewPath @LearnerID={LearnerID},@ProfileID={profileID},@completion_status={completionStatus},@custom_content={customContent},@adaptiverules={adaptiveRules}");
            }

            catch (SqlException ex)
            {

                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    var LearnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == LearnerID);
                    var profileExists = await _context.PersonalizationProfiles.AnyAsync(d => d.ProfileId == profileID);
                    if (!LearnerExists && !profileExists)
                    {
                        ModelState.AddModelError("", "The specified Learner and profile do not exist.");

                    }
                    else
                    {
                        if (!LearnerExists) ModelState.AddModelError("", "The specified Learner does not exist.");
                        else ModelState.AddModelError("", "The specified Profile does not exist.");
                    }
                }
                else if (ex.Message.Contains("PRIMARY KEY constraint"))
                {
                    ModelState.AddModelError("", "This path already Assigned to this Learner");
                }
                else
                {

                    ModelState.AddModelError("", "An error occurred while adding the path. Please try again.");
                }

                return View("Profile",instructor);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("Profile",instructor);
            }
            ViewData
                ["SuccessMessage"] = "Path created successfully!";
            return View("Profile", instructor);
        }
        public async Task<IActionResult> AddPost(int instructorID,  int forumID, string post)
        {

            var instructor = await _context.Instructors.FirstOrDefaultAsync(l => l.InstructorId == instructorID);

            if (instructor == null)
            {

                return RedirectToAction("Profile"); // Redirect to the Profile action if learner does not exist
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec PostINS @instructorID={instructorID},@DiscussionID={forumID},@Post={post}");
            }

            catch (SqlException ex)
            {

                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    var instructorExists = await _context.Instructors.AnyAsync(d => d.InstructorId == instructorID);
                    var forumExists = await _context.DiscussionForums.AnyAsync(d => d.ForumId == forumID);
                    if (!instructorExists && !forumExists)
                    {
                        ModelState.AddModelError("", "The specified Instructor and Forum do not exist.");

                    }
                    else
                    {
                        if (!instructorExists) ModelState.AddModelError("", "The specified Instructor does not exist.");
                        else ModelState.AddModelError("", "The specified Forum does not exist.");
                    }
                }
                else if (ex.Message.Contains("PRIMARY KEY constraint"))
                {
                    ModelState.AddModelError("", "This post already added");
                }
                else
                {

                    ModelState.AddModelError("", "An error occurred while adding the path. Please try again.");
                }

                return View("Profile", instructor);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("Profile", instructor);
            }
            ViewData
                ["SuccessMessage"] = "post added successfully!";
            return View("Profile", instructor);
        }

        public IActionResult ViewScores()
        {
            // Retrieve all learners' scores from the database and map them to the view model
            var learnerScores = _context.Takesassesments
                .Join(_context.Learners, ta => ta.LearnerId, l => l.LearnerId, (ta, l) => new LearnerScoreViewModel
                {
                    LearnerId = l.LearnerId,
                    LearnerName = l.FirstName,
                    LearnerEmail = l.Email,
                    AssessmentId = ta.AssesmentId,
                    ScoredPoints = ta.ScoredPoints
                })
                .ToList();

            return View(learnerScores);
        }

        // Action to view learner scores
        //public IActionResult ViewScores()
        //{
        //    // Retrieve all learners' scores from the database
        //    var learnerScores = _context.Takesassesments
        //        .Join(_context.Learners, ta => ta.LearnerId, l => l.LearnerId, (ta, l) => new
        //        {
        //            l.FirstName,
        //            l.Email,
        //            ta.AssesmentId,
        //            ta.ScoredPoints
        //        })
        //        .ToList();

        //    return View(learnerScores);
        //}

        // Action to update learner's grade
        [HttpPost]
        public IActionResult UpdateGrade(int learnerId, int assessmentId, int scoredPoints)
        {
            try
            {
                // Call the stored procedure to update the grade
                _context.Database.ExecuteSqlRaw("EXEC GradeUpdate @LearnerID = {0}, @AssessmentID = {1}, @points = {2}",
                    learnerId, assessmentId, scoredPoints);

                TempData["SuccessMessage"] = "Grade updated successfully!";
                return RedirectToAction("ViewScores");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the grade.";
                return RedirectToAction("ViewScores");
            }
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
        [HttpGet]
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

        // GET: Display the Create Activity form
        public IActionResult CreateActivity()
        {
            return View();
        }

        // POST: Handle activity creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateActivity(int CourseID, int ModuleID, string ActivityType, string InstructionDetails, int MaxPoints)
        {
            try
            {
                // Check if the module exists
                var moduleExists = _context.Modules.Any(m => m.CourseId == CourseID && m.ModuleId == ModuleID);
                if (!moduleExists)
                {
                    TempData["ErrorMessage"] = "The specified module does not exist.";
                    return View();
                }

                // Execute the stored procedure
                _context.Database.ExecuteSqlRaw("EXEC NewActivity @CourseID, @ModuleID, @ActivityType, @InstructionDetails, @MaxPoints",
                    new SqlParameter("@CourseID", CourseID),
                    new SqlParameter("@ModuleID", ModuleID),
                    new SqlParameter("@ActivityType", ActivityType),
                    new SqlParameter("@InstructionDetails", InstructionDetails),
                    new SqlParameter("@MaxPoints", MaxPoints));

                TempData["SuccessMessage"] = "Activity created successfully!";
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the activity: " + ex.Message;
                return View();
            }
        }

        // GET: Display the Enroll Learner form
        public IActionResult EnrollLearner()
        {
            return View();
        }

        // POST: Handle learner enrollment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnrollLearner(int LearnerID, int CourseID)
        {
            try
            {
                // Check if the learner exists
                var learnerExists = _context.Learners.Any(l => l.LearnerId == LearnerID);
                if (!learnerExists)
                {
                    TempData["ErrorMessage"] = "The specified learner does not exist.";
                    return View();
                }

                // Check if the course exists
                var courseExists = _context.Courses.Any(c => c.CourseId == CourseID);
                if (!courseExists)
                {
                    TempData["ErrorMessage"] = "The specified course does not exist.";
                    return View();
                }

                // Check if the learner is already enrolled
                var alreadyEnrolled = _context.CourseEnrollments.Any(ce => ce.LearnerId == LearnerID && ce.CourseId == CourseID);
                if (alreadyEnrolled)
                {
                    TempData["ErrorMessage"] = "The learner is already enrolled in this course.";
                    return View();
                }

                // Execute the stored procedure
                var connection = _context.Database.GetDbConnection();
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "EXEC Courseregister @LearnerID, @CourseID";
                    command.Parameters.Add(new SqlParameter("@LearnerID", LearnerID));
                    command.Parameters.Add(new SqlParameter("@CourseID", CourseID));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string resultMessage = reader.GetString(0);
                            if (resultMessage.Contains("Registration Failed"))
                            {
                                TempData["ErrorMessage"] = resultMessage;
                                return View();
                            }
                            else
                            {
                                TempData["SuccessMessage"] = resultMessage;
                                return View();
                            }
                        }
                    }
                }

                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while enrolling the learner: " + ex.Message;
                return View();
            }
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorId == id);
        }
    }
}
