using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GamifiedPlatform.Models;

namespace GamifiedPlatform.Controllers
{
    public class QuestsController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public QuestsController(GamifiedPlatformContext context)
        {
            _context = context;
        }

        // GET: Quests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quests.ToListAsync());
        }

        // GET: Quests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests
                .FirstOrDefaultAsync(m => m.QuestId == id);
            if (quest == null)
            {
                return NotFound();
            }

            return View(quest);
        }

        // GET: Quests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestId,DifficultyLevel,Criteria,Description,Title,Deadline")] Quest quest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quest);
        }

        // GET: Quests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }
            return View(quest);
        }

        // POST: Quests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestId,DifficultyLevel,Criteria,Description,Title,Deadline")] Quest quest)
        {
            if (id != quest.QuestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestExists(quest.QuestId))
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
            return View(quest);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests
                .FirstOrDefaultAsync(m => m.QuestId == id);
            if (quest == null)
            {
                return NotFound();
            }

            return View(quest);
        }

        // POST: Quests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quest = await _context.Quests.FindAsync(id);
            if (quest != null)
            {
                _context.Quests.Remove(quest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestExists(int id)
        {
            return _context.Quests.Any(e => e.QuestId == id);
        }

        // GET: Quests/ViewCustom
        public IActionResult ViewCustom()
        {
            var quests = _context.Quests.ToList(); // Fetch all quests
            return View(quests); // Pass data to the custom view
        }

        // POST: Quests/JoinQuest
        [HttpPost]
        public async Task<IActionResult> JoinQuest(int questId)
        {
            // Simulated learner ID - Replace with current logged-in learner ID
            int learnerId = 1; // Replace this with actual learner ID from authentication

            // Check if learner has already joined the quest
            var existingEntry = await _context.LearnersCollaborations
                .FirstOrDefaultAsync(lc => lc.LearnerId == learnerId && lc.QuestId == questId);

            if (existingEntry != null)
            {
                TempData["Error"] = "You have already joined this quest.";
                return RedirectToAction(nameof(ViewCustom));
            }

            // Add the learner to the quest
            var newCollaboration = new LearnersCollaboration
            {
                LearnerId = learnerId,
                QuestId = questId,
                CompletionStatus = "In Progress"
            };

            _context.LearnersCollaborations.Add(newCollaboration);
            await _context.SaveChangesAsync();

            TempData["Success"] = "You have successfully joined the quest!";
            return RedirectToAction(nameof(ViewCustom));
        }
    }
}
