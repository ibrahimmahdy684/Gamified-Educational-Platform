using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using GamifiedPlatform.Models;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace GamifiedPlatform.Controllers
{
    public class LearnersController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public LearnersController(GamifiedPlatformContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard(int id)
        {
            var learner = _context.Learners
                .FirstOrDefault(l => l.UserId == id);

            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }
        public IActionResult GetAllGoals()
        {


            // Execute the stored procedure to get notifications
            var goals = _context.LearningGoals
                .FromSqlRaw("EXEC getAllGoals ")
                .ToList();



            return View(goals);
        }
        public IActionResult GetAllLeaderboards() {
            

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

        // Action to filter leaderboard by LearnerID
        public IActionResult LeaderboardFilter(int LearnerID)
        {

            var learnerIdParam = new SqlParameter("@LearnerID", LearnerID);

            // Execute the stored procedure to get notifications
            var boards = _context.Rankings
                .FromSqlRaw("EXEC LeaderboardFilter @LearnerID", learnerIdParam)
                .ToList();

            // Pass the learnerId to ViewBag for "Back to Profile" button
            ViewBag.LearnerId = LearnerID;

            return View(boards);
        }


        public async Task<IActionResult> AddPost(int learnerID, int forumID, string post)
        {

            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerID);

            if (learner == null)
            {

                return RedirectToAction("Profile"); // Redirect to the Profile action if learner does not exist
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec Post @LearnerID={learnerID},@DiscussionID={forumID},@Post={post}");
            }

            catch (SqlException ex)
            {

                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    var learnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == learnerID);
                    var forumExists = await _context.DiscussionForums.AnyAsync(d => d.ForumId == forumID);
                    if (!learnerExists && !forumExists)
                    {
                        ModelState.AddModelError("", "The specified Learner and Forum do not exist.");

                    }
                    else
                    {
                        if (!learnerExists) ModelState.AddModelError("", "The specified Learner does not exist.");
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

                return View("Profile", learner);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("Profile", learner);
            }
            TempData["SuccessMessage"] = "Post added successfully!";

            return View("Profile", learner);
        }
        public async Task<IActionResult> AddFeedback(int activityID, int learnerID, string emotionalState) 
        {
            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerID);

            if (learner == null)
            {

                return RedirectToAction("Profile"); // Redirect to the Profile action if learner does not exist
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec ViewMyDeviceCharge @LearnerID={learnerID},@ActivityID={activityID},@emotionalstate={emotionalState}");
            }

            catch (SqlException ex)
            {

                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    var learnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == learnerID);
                    var activityExists = await _context.LearningActivities.AnyAsync(d => d.ActivityId == activityID);
                    if (!learnerExists && !activityExists)
                    {
                        ModelState.AddModelError("", "The specified Learner and Activity do not exist.");

                    }
                    else
                    {
                        if (!learnerExists) ModelState.AddModelError("", "The specified Learner does not exist.");
                        else ModelState.AddModelError("", "The specified Activity does not exist.");
                    }
                }
                else if (ex.Message.Contains("PRIMARY KEY constraint"))
                {
                    
                        ModelState.AddModelError("", "This feedback already added");
                    


                }
            }



            catch (Exception ex)
            {

                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("Profile", learner);
            }
            TempData["SuccessMessage"] = "Feedback given successfully!";

            return View("Profile", learner);
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

        public IActionResult ViewAssessments(int learnerId)
        {
            // Prepare the SQL parameter
            var learnerIdParam = new SqlParameter("@LearnerID", learnerId);

            // Execute the stored procedure and get the list of assessments
            var assessments = _context.Assessments
                .FromSqlRaw("EXEC AssessmentAnalysis @LearnerID", learnerIdParam)
                .ToList();

            // Pass the learnerId to ViewBag for the "Back to Profile" button
            ViewBag.UserId = learnerId;

            // Pass the assessments data to the view
            return View(assessments);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInfo(
    int learnerID,
    string firstName,
    string lastName,
    string country,
    string email,
    string culturalBackground,
    IFormFile ProfilePicture)
        {
            // Fetch the learner model to ensure it's not null
            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerID);

            if (learner == null)
            {
                ModelState.AddModelError("", "The specified learner does not exist.");
                return View("Edit", new GamifiedPlatform.Models.Learner()); // Return a new model to prevent null
            }

            string profilePicturePath = learner.ProfilePicturePath; // Default to existing path

            try
            {
                // Handle profile picture upload
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{ProfilePicture.FileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    profilePicturePath = $"/images/{fileName}"; // Update the path for the new picture
                }

                // Update learner information using the stored procedure
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC updateLearnerInfo 
                @learnerID = {learnerID}, 
                @firstName = {firstName}, 
                @lastName = {lastName}, 
                @country = {country}, 
                @email = {email}, 
                @cultural_background = {culturalBackground},
                @profilePicturePath = {profilePicturePath}");

                TempData["SuccessMessage"] = "Learner information updated successfully!";
            }
            catch (Exception ex)
            {
                // Add error to ModelState and pass the learner back to the Edit view
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View("Edit", learner); // Pass the valid learner object to prevent null reference
            }

            // Redirect to the Profile page after successful update
            return RedirectToAction("Edit", new { id = learnerID });
        }

        public IActionResult Profile(int id)
        {
            var learner = _context.Learners.FirstOrDefault(l => l.UserId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        public IActionResult Modules(int courseId)
        {
            // Create the SQL parameter for the stored procedure
            var courseIdParam = new SqlParameter("@courseID", courseId);

            // Execute the stored procedure to get modules for the course
            var modules = _context.Modules
                .FromSqlRaw("EXEC ModuleDifficulty @courseID", courseIdParam)
                .ToList();

            // Retrieve the LearnerId from the Course_enrollment table based on the courseId
            var enrollment = _context.CourseEnrollments
                .FirstOrDefault(ce => ce.CourseId == courseId);

            // If enrollment is found, get the corresponding Learner and UserId
            if (enrollment != null)
            {
                var learner = _context.Learners
                    .FirstOrDefault(l => l.LearnerId == enrollment.LearnerId);

                if (learner != null)
                {
                    // Set the UserId in ViewBag to be used in the view
                    ViewBag.UserId = learner.UserId;
                }
            }

            return View(modules);
        }
        public async Task<IActionResult> AddGoal(int learnerID, int goalID)
        {
            // Fetch the learner data to pass to the view
            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerID);

            if (learner == null)
            {
                TempData["ErrorMessage"] = "The specified learner does not exist.";
                return RedirectToAction("Profile"); // Redirect to the Profile action if learner does not exist
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec AddGoal @learnerID={learnerID},@goalID={goalID}");
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    var learnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == learnerID);
                    if (!learnerExists)
                    {
                        ModelState.AddModelError("", "The specified learner does not exist.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The specified goal does not exist.");
                    }
                }
                else if (ex.Message.Contains("PRIMARY KEY constraint"))
                {
                    ModelState.AddModelError("", "This goal already specified.");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while specifying the goal. Please try again.");
                }

                // Pass the learner model when returning to the view
                return View("Profile", learner);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("Profile", learner);
            }

            TempData["SuccessMessage"] = "Goal added successfully.";
            return View("Profile", learner);
        }

        public IActionResult EnrolledCourses(int id)
        {
            var learner = _context.Learners.FirstOrDefault(l => l.UserId == id);
            if (learner == null)
            {
                return NotFound();
            }

            ViewBag.UserId = id;

            var learnerIdParam = new SqlParameter("@LearnerID", learner.LearnerId);

            // Execute the stored procedure and map the result to the Course entity
            var enrolledCourses = _context.Courses
                .FromSqlRaw("EXEC EnrolledCourses @LearnerID", learnerIdParam)
                .ToList();

            return View(enrolledCourses);
        }




        // GET: Learners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Learners.ToListAsync());
        }

        // GET: Learners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }
       
            // GET: Learners/Create
            public IActionResult Create()
        {
            return View();
        }
        
        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learner);
        }

        [HttpPost]
        public IActionResult EnrollInCourse(int LearnerId, int CourseId)
        {
            if (LearnerId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid learner ID.";
                return RedirectToLearnerProfile(LearnerId);
            }

            // Check if the course exists
            var courseExists = _context.Courses.Any(c => c.CourseId == CourseId);
            if (!courseExists)
            {
                TempData["ErrorMessage"] = "Enrollment failed: The course does not exist.";
                return RedirectToLearnerProfile(LearnerId);
            }

            // Check if the learner is already enrolled in the course
            var alreadyEnrolled = _context.CourseEnrollments
                .Any(ce => ce.LearnerId == LearnerId && ce.CourseId == CourseId);
            if (alreadyEnrolled)
            {
                TempData["ErrorMessage"] = "Enrollment failed: You are already enrolled in this course.";
                return RedirectToLearnerProfile(LearnerId);
            }

            try
            {
                var connection = _context.Database.GetDbConnection();
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "EXEC Courseregister @LearnerID, @CourseID";
                    command.Parameters.Add(new SqlParameter("@LearnerID", LearnerId));
                    command.Parameters.Add(new SqlParameter("@CourseID", CourseId));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string resultMessage = reader.GetString(0);
                            if (resultMessage.Contains("Failed"))
                            {
                                TempData["ErrorMessage"] = resultMessage;
                            }
                            else
                            {
                                TempData["SuccessMessage"] = resultMessage;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Enrollment failed: {ex.Message}";
            }

            return RedirectToLearnerProfile(LearnerId);
        }

        // Helper method for profile redirection
        private IActionResult RedirectToLearnerProfile(int learnerId)
        {
            var learner = _context.Learners.FirstOrDefault(l => l.LearnerId == learnerId);
            if (learner == null)
            {
                TempData["ErrorMessage"] = "Learner not found.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Profile", new { id = learner.UserId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var learner = _context.Learners
                .Include(l => l.User) // Include User data
                .FirstOrDefault(l => l.LearnerId == id);

            if (learner == null)
            {
                return NotFound();
            }

            // Pass the user information to the view if needed
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", learner.UserId);

            return View(learner);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Learner learner, IFormFile ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{ProfilePicture.FileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    learner.ProfilePicturePath = $"/images/{fileName}";
                }

                _context.Update(learner);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile", new { id = learner.UserId });
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", learner.UserId);
            return View(learner);
        }


       
        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learners/Delete/5
        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Execute the DeleteLearner stored procedure
                var connection = _context.Database.GetDbConnection();
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "EXEC DeleteLearner @LearnerID";
                    command.Parameters.Add(new SqlParameter("@LearnerID", id));
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Learner and associated records deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the learner: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ViewPersonalizationProfiles(int learnerId)
        {
            var learnerIdParam = new SqlParameter("@LearnerID", learnerId);

            // Execute the stored procedure to get personalization profiles
            var profiles = _context.PersonalizationProfiles
                .FromSqlRaw("EXEC ViewPersonalizationProfiles @LearnerID", learnerIdParam)
                .ToList();

            // Pass the learnerId to ViewBag for "Back to Profile" button
            ViewBag.LearnerId = learnerId;

            return View(profiles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(int LearnerId)
        {
            try
            {
                // Call the stored procedure to delete the learner and their associated records
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC DeleteLearner @LearnerID = {LearnerId}");

                TempData["SuccessMessage"] = "Your account has been deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the account: {ex.Message}";
                return RedirectToAction("Profile", new { id = LearnerId });
            }

            // Redirect to the Register page after account deletion
            return RedirectToAction("Register", "Account");
        }


        public async Task<IActionResult> DeletePersonalizationProfile(int profileId, int learnerId)
        {
            try
            {
                // Execute the stored procedure to delete the profile
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC DeletePersonalizationProfile @ProfileID = {profileId}");

                TempData["SuccessMessage"] = "Personalization profile deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the profile: {ex.Message}";
            }

            // Redirect back to the ViewPersonalizationProfiles view
            return RedirectToAction(nameof(ViewPersonalizationProfiles), new { learnerId });
        }

        public IActionResult ViewNotifications(int learnerId)
        {
            var learnerIdParam = new SqlParameter("@LearnerID", learnerId);

            // Execute the stored procedure to get notifications
            var notifications = _context.Notifications
                .FromSqlRaw("EXEC ViewNot @LearnerID", learnerIdParam)
                .ToList();

            // Pass the learnerId to ViewBag for "Back to Profile" button
            ViewBag.LearnerId = learnerId;

            return View(notifications);
        }
        
        public IActionResult MonitorPath(int learnerId)
        {
            var learnerIdParam = new SqlParameter("@LearnerID", learnerId);

            // Execute the stored procedure to get notifications
            var Path = _context.LearningPaths
                .FromSqlRaw("EXEC monitorSpecificPath @LearnerID", learnerIdParam)
                .ToList();

            // Pass the learnerId to ViewBag for "Back to Profile" button
            ViewBag.LearnerId = learnerId;

            return View(Path);
        }
        private bool LearnerExists(int id)
        {
            return _context.Learners.Any(e => e.LearnerId == id);
        }
    }
}
