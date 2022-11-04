using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaAPI.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowTimeDetail",
                table: "ShowTime");

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartShift = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndShift = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.ShiftId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShowTimeDetail",
                table: "ShowTime",
                type: "datetime2",
                nullable: true);
        }
    }
}
