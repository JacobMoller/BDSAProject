using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBank.Infrastructure.Migrations
{
    public partial class UserIDChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Projects",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Projects",
                newName: "OwnerId");
        }
    }
}
