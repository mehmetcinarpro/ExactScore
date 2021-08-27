using ExactScore.Data.Entities;
using System.Collections.Generic;

namespace ExactScore.Models
{
    public class HomeViewModel
    {
        public IEnumerable<StandingsItemViewModel> Standings { get; set; }
        public IEnumerable<Fixture> Fixtures { get; set; }
    }
}
