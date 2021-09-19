using ExactScore.Data.Entities;
using System;

namespace ExactScore.Models
{
    public class PredictionViewModel
    {
        public int FixtureId { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public int? HomeGoal { get; set; }
        public int? AwayGoal { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
    }
}
