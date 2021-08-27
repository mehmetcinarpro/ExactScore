using ExactScore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExactScore.Data.Repositories
{
    public interface IStandingsRepository
    {
        Task<IEnumerable<StandingsItemViewModel>> GetStandings();
    }
}
