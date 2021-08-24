using System;

namespace ExactScore.Data.Entities
{
    public class Fixture
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }
        public int? HomeGoal { get; set; }
        public int? AwayGoal { get; set; }
        public int RoundId { get; set; }
        public Round Round { get; set; }
    }
}
