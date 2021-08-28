using System.Collections.Generic;

namespace ExactScore.Models
{
    public class HomeViewModel
    {
        public IEnumerable<StandingsItemViewModel> Standings { get; set; }
        public IEnumerable<PredictionViewModel> MissingPredictions { get; set; }
        public IEnumerable<PredictionViewModel> InProgressPredictions { get; set; }
    }
}
