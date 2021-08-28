using System;

namespace ExactScore.Models
{
    public class PredictionViewModel
    {
        public int FixtureId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeGoal { get; set; }
        public int? AwayGoal { get; set; }
        public DateTime Date { get; set; }
    }
}
