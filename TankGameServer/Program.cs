using Microsoft.EntityFrameworkCore;
using TankGameServer.DataService;
using TankGameServer.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddSignalR();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Frontend-porten
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();
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
