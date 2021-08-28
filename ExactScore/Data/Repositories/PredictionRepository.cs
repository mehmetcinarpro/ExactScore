using ExactScore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExactScore.Data.Repositories
{
    public class PredictionRepository : IPredictionRepository
    {
        private readonly ApplicationDbContext _context;

        public PredictionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PredictionViewModel>> GetInProgressPredictions(string userId)
        {
            var myPredictions = await _context.Predictions.Include(p => p.Fixture).Include(p => p.Fixture.HomeTeam).Include(p => p.Fixture.AwayTeam)
                .Where(p => p.IdentityUserId == userId && p.Point == null).ToListAsync();

            return myPredictions.Select(p => new PredictionViewModel
            {
                FixtureId = p.Fixture.Id,
                HomeTeam = p.Fixture.HomeTeam.Name,
                AwayTeam = p.Fixture.AwayTeam.Name,
                HomeGoal = p.HomeGoal,
                AwayGoal = p.AwayGoal,
                Date = p.Fixture.Date
            });
        }

        public async Task<IEnumerable<PredictionViewModel>> GetMissingPredictions(string userId)
        {
            var fixures = await _context.Fixtures.Include(f => f.HomeTeam).Include(f => f.AwayTeam).Where(f => f.Date > System.DateTime.Now.AddMinutes(10)).ToListAsync();
            var myPredictions = await _context.Predictions.Where(p => p.IdentityUserId == userId).ToListAsync();
            var missingPredictions = fixures.Except(myPredictions.Select(p => p.Fixture));

            return missingPredictions.Select(p => new PredictionViewModel
            {
                FixtureId = p.Id,
                HomeTeam = p.HomeTeam.Name,
                AwayTeam = p.AwayTeam.Name,
                Date = p.Date
            });
        }
    }
}
