using ExactScore.Data;
using ExactScore.Data.Entities;
using ExactScore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        // GET: Fixtures/Create
        public IActionResult Create()
        {
            ViewData["AwayTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name");
            ViewData["HomeTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name");
            ViewData["RoundId"] = new SelectList(_context.Rounds.Where(r => !r.Closed), "Id", "Name");
            return View();
        }

        // POST: Fixtures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,HomeTeamId,AwayTeamId,RoundId")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", fixture.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", fixture.HomeTeamId);
            ViewData["RoundId"] = new SelectList(_context.Rounds.Where(r => !r.Closed), "Id", "Name", fixture.RoundId);
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
            ViewData["AwayTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", fixture.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", fixture.HomeTeamId);
            ViewData["RoundId"] = new SelectList(_context.Rounds.Where(r => !r.Closed), "Id", "Name", fixture.RoundId);
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
            ViewData["AwayTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", fixture.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", fixture.HomeTeamId);
            ViewData["RoundId"] = new SelectList(_context.Rounds.Where(r => !r.Closed), "Id", "Name", fixture.RoundId);
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


        // GET: Fixtures/Prediction/5
        public async Task<IActionResult> Prediction(int id)
        {
            var fixture = await _context.Fixtures.Include(f => f.HomeTeam).Include(f => f.AwayTeam).SingleOrDefaultAsync(f => f.Id == id);
            if (fixture == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var prediction = await _context.Predictions.Include(f => f.Fixture).Include(f => f.Fixture.HomeTeam).Include(f => f.Fixture.AwayTeam)
                .SingleOrDefaultAsync(f => f.FixtureId == id && f.IdentityUserId == userId);
            // new prediction
            ViewData["Goals"] = new SelectList(Enumerable.Range(0, 10).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            if (prediction != null)
            {
                return View(new PredictionViewModel
                {
                    FixtureId = prediction.Fixture.Id,
                    HomeTeam = prediction.Fixture.HomeTeam,
                    AwayTeam = prediction.Fixture.AwayTeam,
                    HomeGoal = prediction.HomeGoal,
                    AwayGoal = prediction.AwayGoal,
                    Date = prediction.Fixture.Date
                });
            }

            // update prediction
            return View(new PredictionViewModel
            {
                FixtureId = fixture.Id,
                HomeTeam = fixture.HomeTeam,
                AwayTeam = fixture.AwayTeam,
                Date = fixture.Date
            });


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prediction([Bind("FixtureId,HomeGoal,AwayGoal")] PredictionViewModel prediction)
        {
            if (ModelState.IsValid)
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var entity = await _context.Predictions.Include(f => f.Fixture).Include(f => f.Fixture.HomeTeam).Include(f => f.Fixture.AwayTeam)
                    .SingleOrDefaultAsync(f => f.FixtureId == prediction.FixtureId && f.IdentityUserId == userId);

                if (entity != null)
                {
                    entity.HomeGoal = prediction.HomeGoal.Value;
                    entity.AwayGoal = prediction.AwayGoal.Value;
                }
                else
                {
                    var newEntity = new Prediction
                    {
                        IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        FixtureId = prediction.FixtureId,
                        HomeGoal = prediction.HomeGoal.Value,
                        AwayGoal = prediction.AwayGoal.Value
                    };
                    _context.Predictions.Add(newEntity);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Home");
            }
            ViewData["Goals"] = new SelectList(Enumerable.Range(0, 10).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }), "Value", "Text");
            return View(prediction);
        }
    }
}
