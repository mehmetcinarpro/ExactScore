using ExactScore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ExactScore.Data.Repositories
{
    public class StandingsRepository : IStandingsRepository
    {
        private readonly ApplicationDbContext _context;

        public StandingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<StandingsItemViewModel> Standings => (_context.Predictions.Include(p => p.IdentityUser).Where(p => p.Point != null).AsEnumerable().GroupBy(g => g.IdentityUser.Email)
                .ToDictionary(d => d.Key, g => new StandingsItemViewModel
                {
                    UserName = g.Key,
                    Played = g.Count(),
                    ExactScore = g.Count(p => (int)p.Point == 3),
                    Win = g.Count(p => (int)p.Point == 1),
                    Lost = g.Count(p => (int)p.Point == 0),
                    Point = g.Sum(p => (int)p.Point)
                })).OrderByDescending(p => p.Value.Point).Select(p => p.Value).ToList();
    }
}
