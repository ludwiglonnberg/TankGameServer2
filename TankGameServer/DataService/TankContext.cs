using System;
using Microsoft.EntityFrameworkCore;
using TankGameServer.Models;
namespace TankGameServer.DataService
{
    public class TankContext : DbContext
    {

        public TankContext(DbContextOptions<TankContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

    }
}
