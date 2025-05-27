namespace TankGameServer.Models
{
    public class User
    {
        public int UserId {  get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int GamesPlayed { get; set; } = 0;


    }
}
