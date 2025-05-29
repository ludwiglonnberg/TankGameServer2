using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using TankGameServer.DataService;
using TankGameServer.Models;
using Microsoft.AspNetCore.Identity;
using Azure.Core;

namespace TankGameServer.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly TankContext _context;

        public UserController(TankContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] TankLoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == request.Email);

            if (user == null)
                return Unauthorized("Wrong email or password");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Wrong email or password");

            return Ok(new { user.UserId, user.Username });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] TankRegisterRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Username is already in use.");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,

            };

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Success");
        }
        [HttpGet("stats/{userId}")]
        public IActionResult GetUserStats(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found");

            var stats = new
            {
                user.Username,
                user.Wins,
                user.Losses,
                user.GamesPlayed
            };

            return Ok(stats);
        }
        [HttpPost("updateStats")]
        public IActionResult UpdateStats([FromBody] TankUpdateStatsRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == request.UserId);
            if (user == null)
                return NotFound("User not found");

            // Uppdatera statistik
            user.GamesPlayed += 1;
            if (request.Won)
            {
                user.Wins += 1;
            }
            else
            {
                user.Losses += 1;
            }

            _context.SaveChanges();

            return Ok("Statistics updated!");
        }
        [HttpGet("list")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.Select(u => new
            {
                u.UserId,
                u.Username,
                u.Email,
                u.Wins,
                u.Losses,
                u.GamesPlayed
            }).ToList();

            return Ok(users);
        }



    }
}