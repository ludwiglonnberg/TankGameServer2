using Microsoft.EntityFrameworkCore;
using TankGameServer.DataService;
using TankGameServer.Hubs;
using Npgsql.EntityFrameworkCore.PostgreSQL; // Add this using directive for UseNpgsql extension method  
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TankContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();


var corsOrigin = builder.Configuration["CORS_ORIGIN"] ?? "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000"; // fallback

app.Urls.Add($"http://*:{port}");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TankContext>();
    db.Database.Migrate(); 
}

app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<GameHub>("/gamehub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
