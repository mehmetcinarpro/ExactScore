namespace ExactScore.Data.Entities
{
    public class Round
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public bool Closed { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
