using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBank.Infrastructure.Migrations
{
    public partial class RenameUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Projects",
                newName: "SupervisorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupervisorId",
                table: "Projects",
                newName: "UserId");
        }
    }
}
