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
    public class LearningGoalsController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public LearningGoalsController(GamifiedPlatformContext context)
        {
            _context = context;
        }

        // GET: LearningGoals
        public async Task<IActionResult> Index()
        {
            return View(await _context.LearningGoals.ToListAsync());
        }

        public async Task<IActionResult> AddGoal(int learnerID,int goalID) {
            var LearnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == learnerID);
            if (!LearnerExists)
            {
                ModelState.AddModelError("", "The specified learner does not exist.");
                return View("AddGoal");
            }
            var GoalExists = await _context.LearningGoals.AnyAsync(d => d.Id == goalID);
            if (!GoalExists)
            {
                ModelState.AddModelError("", "The specified goal does not exist.");
                return View("AddGoal");
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"Exec AddGoal @learnerID={learnerID},@goalID={goalID} ");
            return RedirectToAction("Index");
        }
        // GET: LearningGoals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningGoal = await _context.LearningGoals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learningGoal == null)
            {
                return NotFound();
            }

            return View(learningGoal);
        }

        // GET: LearningGoals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LearningGoals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,Deadline,Description")] LearningGoal learningGoal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learningGoal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learningGoal);
        }

        // GET: LearningGoals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningGoal = await _context.LearningGoals.FindAsync(id);
            if (learningGoal == null)
            {
                return NotFound();
            }
            return View(learningGoal);
        }

        // POST: LearningGoals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,Deadline,Description")] LearningGoal learningGoal)
        {
            if (id != learningGoal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learningGoal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearningGoalExists(learningGoal.Id))
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
            return View(learningGoal);
        }

        // GET: LearningGoals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningGoal = await _context.LearningGoals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learningGoal == null)
            {
                return NotFound();
            }

            return View(learningGoal);
        }

        // POST: LearningGoals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningGoal = await _context.LearningGoals.FindAsync(id);
            if (learningGoal != null)
            {
                _context.LearningGoals.Remove(learningGoal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearningGoalExists(int id)
        {
            return _context.LearningGoals.Any(e => e.Id == id);
        }
    }
}
