using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaAPI.Migrations
{
    public partial class UpdateTableMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Movie");
        }
    }
}
