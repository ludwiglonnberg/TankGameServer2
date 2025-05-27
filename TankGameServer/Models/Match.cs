namespace TankGameServer.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public DateTime Date { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }

    }
}
