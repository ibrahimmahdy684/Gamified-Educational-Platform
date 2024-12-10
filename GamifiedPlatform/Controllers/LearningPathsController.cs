using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectt.Models;

namespace projectt.Controllers
{
    public class LearningPathsController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public LearningPathsController(GamifiedPlatformContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> MonitorPath(int learnerID,int pathID) {
            var path = await _context.LearningPaths
     .FromSqlRaw($"Exec monitorSpecificPath @learnerID={learnerID}, @pathID={pathID}")
     .ToListAsync();
            return View(path);
        }
        public async Task<IActionResult> AddPath(int LearnerID, int profileID, string completionStatus, string customContent, string adaptiveRules) 
        {
            var LearnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == LearnerID);
            if (!LearnerExists)
            {
                ModelState.AddModelError("", "The specified learner does not exist.");
                return View("AddPath");
            }
            var ProfileExists = await _context.PersonalizationProfiles.AnyAsync(d => d.ProfileId == profileID);
            if (!ProfileExists)
            {
                ModelState.AddModelError("", "The specified profile does not exist.");
                return View("AddPath");
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"Exec NewPath @LearnerID={LearnerID},@ProfileID={profileID},@completion_status={completionStatus},@custom_content={customContent},@adaptiverules={adaptiveRules}");
            return RedirectToAction("Index"); 
        }
        // GET: LearningPaths
        public async Task<IActionResult> Index()
        {
            var gamifiedPlatformContext = _context.LearningPaths.Include(l => l.PersonalizationProfile);
            return View(await gamifiedPlatformContext.ToListAsync());
        }

        // GET: LearningPaths/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningPath = await _context.LearningPaths
                .Include(l => l.PersonalizationProfile)
                .FirstOrDefaultAsync(m => m.PathId == id);
            if (learningPath == null)
            {
                return NotFound();
            }

            return View(learningPath);
        }

        // GET: LearningPaths/Create
        public IActionResult Create()
        {
            ViewData["LearnerId"] = new SelectList(_context.PersonalizationProfiles, "LearnerId", "LearnerId");
            return View();
        }

        // POST: LearningPaths/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PathId,LearnerId,ProfileId,CompletionStatus,CustomContent,AdaptiveRules")] LearningPath learningPath)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learningPath);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LearnerId"] = new SelectList(_context.PersonalizationProfiles, "LearnerId", "LearnerId", learningPath.LearnerId);
            return View(learningPath);
        }

        // GET: LearningPaths/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningPath = await _context.LearningPaths.FindAsync(id);
            if (learningPath == null)
            {
                return NotFound();
            }
            ViewData["LearnerId"] = new SelectList(_context.PersonalizationProfiles, "LearnerId", "LearnerId", learningPath.LearnerId);
            return View(learningPath);
        }

        // POST: LearningPaths/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PathId,LearnerId,ProfileId,CompletionStatus,CustomContent,AdaptiveRules")] LearningPath learningPath)
        {
            if (id != learningPath.PathId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learningPath);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearningPathExists(learningPath.PathId))
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
            ViewData["LearnerId"] = new SelectList(_context.PersonalizationProfiles, "LearnerId", "LearnerId", learningPath.LearnerId);
            return View(learningPath);
        }

        // GET: LearningPaths/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningPath = await _context.LearningPaths
                .Include(l => l.PersonalizationProfile)
                .FirstOrDefaultAsync(m => m.PathId == id);
            if (learningPath == null)
            {
                return NotFound();
            }

            return View(learningPath);
        }

        // POST: LearningPaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningPath = await _context.LearningPaths.FindAsync(id);
            if (learningPath != null)
            {
                _context.LearningPaths.Remove(learningPath);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearningPathExists(int id)
        {
            return _context.LearningPaths.Any(e => e.PathId == id);
        }
    }
}
