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
    public class EmotionalFeedbacksController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public EmotionalFeedbacksController(GamifiedPlatformContext context)
        {
            _context = context;
        }

        // GET: EmotionalFeedbacks
        public async Task<IActionResult> Index()
        {
            var gamifiedPlatformContext = _context.EmotionalFeedbacks.Include(e => e.Learner);
            return View(await gamifiedPlatformContext.ToListAsync());
        }
        public async Task<IActionResult> AddFeedback(int activityID,int learnerID,string feedback)
        {
            try {
                await _context.Database.ExecuteSqlInterpolatedAsync($"Exec ViewMyDeviceCharge @ActivityID={activityID},@LearnerID={learnerID},@timestamp={DateTime.Now},@emotionalstate={feedback}");
            }
            catch (SqlException ex)
            {

                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    var LearnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == learnerID);
                    var activityExists = await _context.LearningActivities.AnyAsync(d => d.ActivityId == activityID);
                    if (!LearnerExists && !activityExists)
                    {
                        ModelState.AddModelError("", "The specified Learner and Activity do not exist.");

                    }
                    else
                    {
                        if (!LearnerExists) ModelState.AddModelError("", "The specified Learner does not exist.");
                        else ModelState.AddModelError("", "The specified Activity does not exist.");
                    }
                }
                else if (ex.Message.Contains("PRIMARY KEY constraint"))
                {
                    ModelState.AddModelError("", "This feedback already added");
                }
                else
                {

                    ModelState.AddModelError("", "An error occurred while giving the feedback. Please try again.");
                }

                return View("AddFeedback");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                return View("AddFeedback");
            }
            return RedirectToAction("Index");
        }
        // GET: EmotionalFeedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emotionalFeedback = await _context.EmotionalFeedbacks
                .Include(e => e.Learner)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (emotionalFeedback == null)
            {
                return NotFound();
            }

            return View(emotionalFeedback);
        }

        // GET: EmotionalFeedbacks/Create
        public IActionResult Create()
        {
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            return View();
        }

        // POST: EmotionalFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackId,LearnerId,Timestamp,EmotionalState")] EmotionalFeedback emotionalFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emotionalFeedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", emotionalFeedback.LearnerId);
            return View(emotionalFeedback);
        }

        // GET: EmotionalFeedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emotionalFeedback = await _context.EmotionalFeedbacks.FindAsync(id);
            if (emotionalFeedback == null)
            {
                return NotFound();
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", emotionalFeedback.LearnerId);
            return View(emotionalFeedback);
        }

        // POST: EmotionalFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackId,LearnerId,Timestamp,EmotionalState")] EmotionalFeedback emotionalFeedback)
        {
            if (id != emotionalFeedback.FeedbackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emotionalFeedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmotionalFeedbackExists(emotionalFeedback.FeedbackId))
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
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", emotionalFeedback.LearnerId);
            return View(emotionalFeedback);
        }

        // GET: EmotionalFeedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emotionalFeedback = await _context.EmotionalFeedbacks
                .Include(e => e.Learner)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (emotionalFeedback == null)
            {
                return NotFound();
            }

            return View(emotionalFeedback);
        }

        // POST: EmotionalFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emotionalFeedback = await _context.EmotionalFeedbacks.FindAsync(id);
            if (emotionalFeedback != null)
            {
                _context.EmotionalFeedbacks.Remove(emotionalFeedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmotionalFeedbackExists(int id)
        {
            return _context.EmotionalFeedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
