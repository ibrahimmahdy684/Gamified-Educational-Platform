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
    public class LeaderboardsController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public LeaderboardsController(GamifiedPlatformContext context)
        {
            _context = context;
        }

        // GET: Leaderboards
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leaderboards.ToListAsync());
        }
       /* public async Task<IActionResult> Index(int learnerID)
        {
            var LearnerExists = await _context.Learners.AnyAsync(d => d.LearnerId == learnerID);
            if (!LearnerExists)
            {
                ModelState.AddModelError("", "The specified Learner does not exist.");
                return View("Index");
            }
            var Leaderboard = await _context.Leaderboards.FromSqlRaw($"Exec LeaderboardFilter @LearnerID={learnerID}").ToListAsync();
            return View(Leaderboard);
        }*/
        // GET: Leaderboards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _context.Leaderboards
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (leaderboard == null)
            {
                return NotFound();
            }

            return View(leaderboard);
        }

        // GET: Leaderboards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leaderboards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoardId,Season")] Leaderboard leaderboard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaderboard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaderboard);
        }

        // GET: Leaderboards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _context.Leaderboards.FindAsync(id);
            if (leaderboard == null)
            {
                return NotFound();
            }
            return View(leaderboard);
        }

        // POST: Leaderboards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BoardId,Season")] Leaderboard leaderboard)
        {
            if (id != leaderboard.BoardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaderboard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaderboardExists(leaderboard.BoardId))
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
            return View(leaderboard);
        }

        // GET: Leaderboards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboard = await _context.Leaderboards
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (leaderboard == null)
            {
                return NotFound();
            }

            return View(leaderboard);
        }

        // POST: Leaderboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaderboard = await _context.Leaderboards.FindAsync(id);
            if (leaderboard != null)
            {
                _context.Leaderboards.Remove(leaderboard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaderboardExists(int id)
        {
            return _context.Leaderboards.Any(e => e.BoardId == id);
        }
    }
}
