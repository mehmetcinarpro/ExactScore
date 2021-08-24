namespace ExactScore.Data.Entities
{
    public class Season
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int LeagueId { get; set; }
        public League League { get; set; }
    }
}
