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

        public async Task<IEnumerable<PredictionViewModel>> GetMissingPredictions(string userId)
        {
            var fixures = await _context.Fixtures.Include(f => f.HomeTeam).Include(f => f.AwayTeam).ToListAsync();
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
