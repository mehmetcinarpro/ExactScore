using ExactScore.Data;
using ExactScore.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExactScore.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PredictionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Predictions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Predictions
                .Include(p => p.Fixture)
                .Include(p => p.Fixture.Round)
                .Include(p => p.IdentityUser)
                .Include(p => p.Fixture.HomeTeam)
                .Include(p => p.Fixture.AwayTeam);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Predictions/Create
        public IActionResult Create()
        {
            ViewData["FixtureId"] = new SelectList(_context.Fixtures.Include(f => f.AwayTeam).Include(f => f.HomeTeam), "Id", "Name");
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FixtureId,UserName,HomeGoal,AwayGoal,Point,IdentityUserId")] Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prediction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FixtureId"] = new SelectList(_context.Fixtures.Include(f => f.AwayTeam).Include(f => f.HomeTeam), "Id", "Name", prediction.FixtureId);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", prediction.IdentityUserId);
            return View(prediction);
        }

        // GET: Predictions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _context.Predictions
                .Include(p => p.Fixture)
                .Include(p => p.Fixture.HomeTeam)
                .Include(p => p.Fixture.AwayTeam)
                .Include(p => p.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prediction == null)
            {
                return NotFound();
            }
            ViewData["FixtureId"] = new SelectList(_context.Fixtures.Include(f => f.AwayTeam).Include(f => f.HomeTeam), "Id", "Name", prediction.FixtureId);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", prediction.IdentityUserId);
            return View(prediction);
        }

        // POST: Predictions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FixtureId,UserName,HomeGoal,AwayGoal,Point,IdentityUserId")] Prediction prediction)
        {
            if (id != prediction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prediction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PredictionExists(prediction.Id))
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
            ViewData["FixtureId"] = new SelectList(_context.Fixtures.Include(f => f.AwayTeam).Include(f => f.HomeTeam), "Id", "Name", prediction.FixtureId);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", prediction.IdentityUserId);
            return View(prediction);
        }

        // GET: Predictions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _context.Predictions
                .Include(p => p.Fixture)
                .Include(p => p.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prediction == null)
            {
                return NotFound();
            }

            return View(prediction);
        }

        // POST: Predictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PredictionExists(int id)
        {
            return _context.Predictions.Any(e => e.Id == id);
        }
    }
}
