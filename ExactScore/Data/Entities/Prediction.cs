namespace ExactScore.Data.Entities
{
    public class Prediction
    {
        public int Id { get; set; }
        public int FixtureId { get; set; }
        public Fixture Fixture { get; set; }
        public string UserName { get; set; }
        public int HomeGoal { get; set; }
        public int AwayGoal { get; set; }
        public int? Point { get; set; }
    }
}
