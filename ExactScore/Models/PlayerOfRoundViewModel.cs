using System.Collections.Generic;

namespace ExactScore.Models
{
    public class PlayerOfRoundViewModel
    {
        public string Username { get; set; }
        public IEnumerable<PredictionViewModel> Predictions { get; set; }
    }
}
