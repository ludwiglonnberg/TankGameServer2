using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TankGameServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GamesPlayed",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamesPlayed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "Users");
        }
    }
}
