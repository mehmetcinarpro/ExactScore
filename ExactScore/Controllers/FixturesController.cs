using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExactScore.Data;
using ExactScore.Data.Entities;

namespace ExactScore.Controllers
{
    public class FixturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FixturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fixtures
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fixtures.Include(f => f.AwayTeam).Include(f => f.HomeTeam).Include(f => f.Round);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fixtures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures
                .Include(f => f.AwayTeam)
                .Include(f => f.HomeTeam)
                .Include(f => f.Round)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // GET: Fixtures/Create
        public IActionResult Create()
        {
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id");
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id");
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id");
            return View();
        }

        // POST: Fixtures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,HomeTeamId,AwayTeamId,HomeGoal,AwayGoal,RoundId")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id", fixture.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id", fixture.HomeTeamId);
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id", fixture.RoundId);
            return View(fixture);
        }

        // GET: Fixtures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures.FindAsync(id);
            if (fixture == null)
            {
                return NotFound();
            }
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id", fixture.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id", fixture.HomeTeamId);
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id", fixture.RoundId);
            return View(fixture);
        }

        // POST: Fixtures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,HomeTeamId,AwayTeamId,HomeGoal,AwayGoal,RoundId")] Fixture fixture)
        {
            if (id != fixture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixtureExists(fixture.Id))
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
            ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id", fixture.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id", fixture.HomeTeamId);
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id", fixture.RoundId);
            return View(fixture);
        }

        // GET: Fixtures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures
                .Include(f => f.AwayTeam)
                .Include(f => f.HomeTeam)
                .Include(f => f.Round)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // POST: Fixtures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fixture = await _context.Fixtures.FindAsync(id);
            _context.Fixtures.Remove(fixture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FixtureExists(int id)
        {
            return _context.Fixtures.Any(e => e.Id == id);
        }
    }
}
