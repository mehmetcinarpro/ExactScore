using ExactScore.Models;
using System.Collections.Generic;

namespace ExactScore.Data.Repositories
{
    public interface IStandingsRepository
    {
        IEnumerable<StandingsItemViewModel> Standings { get; }
    }
}
