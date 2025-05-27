namespace TankGameServer.Models
{
    public class TankUpdateStatsRequest
    {
        public int UserId { get; set; }
        public bool Won { get; set; }
    }
}
