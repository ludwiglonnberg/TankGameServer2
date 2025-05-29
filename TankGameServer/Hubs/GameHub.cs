using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TankGameServer.Models;
namespace TankGameServer.Hubs;
public class GameHub : Hub
{
    
    private static Dictionary<string, string> connectedPlayers = new();
    private static Dictionary<string, string> connectionToUserId = new();

    private static Dictionary<string, int> playerHealth = new();
    private static readonly (int x, int y)[] spawnPoints = new (int, int)[]
{
    (100, 100),
    (900,600)
};
    private async Task CheckGameOver()
    {
        var alivePlayers = playerHealth
            .Where(p => p.Value > 0)
            .Select(p => p.Key)
            .ToList();

        if (alivePlayers.Count == 1)
        {
            var winnerConnectionId = alivePlayers[0];
            var winnerName = connectedPlayers[winnerConnectionId] ?? "Unknown";
            var winnerUserId = connectionToUserId[winnerConnectionId];

            Console.WriteLine($"Game Over! Winner: {winnerName}");

            await Clients.All.SendAsync("GameOver", winnerUserId, winnerName);
        }
        else if (alivePlayers.Count == 0)
        {
            await Clients.All.SendAsync("GameOver", null, "No winner");
        }
    }


    public async Task SendPlayerPosition(string playerId, float x, float y, float angle,float turretAngle)
    {
        // Spara position i någon state och skicka till alla andra
        await Clients.Others.SendAsync("ReceivePlayerPosition", playerId, x, y, angle, turretAngle);
    }

    public async Task PlayerHit(string playerId, int damage)
    {
        if (!playerHealth.ContainsKey(playerId)) return;

        playerHealth[playerId] -= damage;

        await Clients.All.SendAsync("PlayerHealthUpdated", playerId, playerHealth[playerId]);

        if (playerHealth[playerId] <= 0)
        {
            await Clients.All.SendAsync("PlayerDied", playerId);

            await CheckGameOver();
        }

    }
    public Task BulletFired(string playerId, float x, float y, float dx, float dy, float angle)
    {
        return Clients.Others.SendAsync("ReceiveBullet", playerId, x, y, dx, dy, angle);
    }

    public async Task SendExplosion(float x, float y)
    {
        await Clients.Others.SendAsync("ReceiveExplosion", x, y);
    }

    public override async Task OnConnectedAsync()
    {
        
        Console.WriteLine($"Player connected: {Context.ConnectionId}");

        // Skicka connectionId till klienten så den vet sin egen id
        await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayerId", Context.ConnectionId);

        await base.OnConnectedAsync();
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        connectedPlayers.Remove(Context.ConnectionId);
        playerHealth.Remove(Context.ConnectionId);
        connectionToUserId.Remove(Context.ConnectionId);

        Console.WriteLine($"Player disconnected: {Context.ConnectionId}");

        await base.OnDisconnectedAsync(exception);
    }



    // Anropas från klienten när spelaren är redo att spela
    public async Task RegisterPlayer(string playerName, string userId)
    {
        connectedPlayers[Context.ConnectionId] = playerName;
        connectionToUserId[Context.ConnectionId] = userId;
        playerHealth[Context.ConnectionId] = 100;
        Console.WriteLine($"Player registered: {playerName} (UserId: {userId})");

        await TryMatchPlayers();
    }


    private async Task TryMatchPlayers()
    {
        var spawn1 = spawnPoints[0];
        var spawn2 = spawnPoints[1];

        var readyPlayers = connectedPlayers
            .Where(p => p.Value != null)
            .Select(p => p.Key)
            .ToList();

        while (readyPlayers.Count >= 2)
        {
            var player1 = readyPlayers[0];
            var player2 = readyPlayers[1];

            var t1 = Clients.Client(player1).SendAsync("MatchStarted", spawn1.x, spawn1.y);

            var t2 = Clients.Client(player2).SendAsync("MatchStarted", spawn2.x, spawn2.y);

            await Task.WhenAll(t1, t2);

            readyPlayers.RemoveRange(0, 2);
        }
    }
}
