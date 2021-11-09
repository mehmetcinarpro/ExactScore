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
                .Include(p => p.IdentityUser)
                .Where(p => p.IdentityUserId == userId && p.Point == null).ToListAsync();

            return myPredictions.Select(p => new PredictionViewModel
            {
                FixtureId = p.Fixture.Id,
                HomeTeam = p.Fixture.HomeTeam,
                AwayTeam = p.Fixture.AwayTeam,
                HomeGoal = p.HomeGoal,
                AwayGoal = p.AwayGoal,
                Date = p.Fixture.Date,
                Username = p.IdentityUser.UserName
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
                HomeTeam = p.HomeTeam,
                AwayTeam = p.AwayTeam,
                Date = p.Date
            });
        }

        public async Task<PlayerOfRoundViewModel> GetPlayerOfRound()
        {
            var lastRound = await _context.Rounds.OrderByDescending(r => r.OrderNumber).FirstAsync(r => r.Closed);
            var predictions = _context.Predictions.Include(p => p.Fixture).Include(p => p.Fixture.HomeTeam).Include(p => p.Fixture.AwayTeam)
                .Where(p => p.Fixture.RoundId == lastRound.Id).AsEnumerable().GroupBy(p => p.IdentityUserId).ToDictionary(g => g.Key, g => g.ToList());
            var bestPlayer = predictions.ToDictionary(g => g.Key, g => g.Value.Sum(m => m.Point)).OrderByDescending(p => p.Value).First();
            var bestPlayerPredictions = predictions.First(p => p.Key == bestPlayer.Key).Value;

            return new PlayerOfRoundViewModel
            {
                Username = bestPlayerPredictions.First().IdentityUser.UserName,
                Predictions = bestPlayerPredictions.Select(p => new PredictionViewModel
                {
                    FixtureId = p.FixtureId,
                    HomeTeam = p.Fixture.HomeTeam,
                    AwayTeam = p.Fixture.AwayTeam,
                    HomeGoal = p.HomeGoal,
                    AwayGoal = p.AwayGoal,
                    Date = p.Fixture.Date,
                    Point = p.Point
                })
            };
        }
    }
}
