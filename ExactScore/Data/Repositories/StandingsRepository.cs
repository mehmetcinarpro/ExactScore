using ExactScore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExactScore.Data.Repositories
{
    public class StandingsRepository : IStandingsRepository
    {
        private readonly ApplicationDbContext _context;

        public StandingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StandingsItemViewModel>> GetStandings()
        {
            return (await _context.Predictions
                .Include(p => p.IdentityUser)
                .Where(p => p.Point != null)
                .ToListAsync())
                .GroupBy(g => g.IdentityUser.UserName)
                .ToDictionary(d => d.Key, g => new StandingsItemViewModel
                {
                    UserName = g.Key,
                    Played = g.Count(),
                    ExactScore = g.Count(p => (int)p.Point == 3),
                    Win = g.Count(p => (int)p.Point == 1),
                    Lost = g.Count(p => (int)p.Point == 0),
                    Point = g.Sum(p => (int)p.Point)
                })
                .OrderByDescending(p => p.Value.Point).Select(p => p.Value);
        }

        public async Task RefreshStandings()
        {
            var predictions = await _context.Predictions.Include(p => p.Fixture).Where(p => p.Point == null).ToListAsync();
            foreach (var prediction in predictions)
            {
                var fixture = prediction.Fixture;
                if (fixture.HomeGoal == null && fixture.AwayGoal == null)
                {
                    continue;
                }

                if ((prediction.HomeGoal > prediction.AwayGoal && fixture.HomeGoal > fixture.AwayGoal)
                    || (prediction.HomeGoal < prediction.AwayGoal && fixture.HomeGoal < fixture.AwayGoal)
                    || (prediction.HomeGoal == prediction.AwayGoal && fixture.HomeGoal == fixture.AwayGoal))
                {
                    if (prediction.HomeGoal == fixture.HomeGoal && prediction.AwayGoal == fixture.AwayGoal)
                    {
                        prediction.Point = 3;
                    }
                    else
                    {
                        prediction.Point = 1;
                    }
                }
                else
                {
                    prediction.Point = 0;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
