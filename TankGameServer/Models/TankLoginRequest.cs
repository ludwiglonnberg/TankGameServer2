namespace TankGameServer.Models
{
    public class TankLoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
