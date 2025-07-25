using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eciton.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedMaxFailedAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxFailedAccessAttempts",
                table: "AppUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxFailedAccessAttempts",
                table: "AppUsers");
        }
    }
}
