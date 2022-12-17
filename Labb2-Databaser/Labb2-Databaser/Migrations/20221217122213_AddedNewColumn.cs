using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb2Databaser.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sidor",
                table: "Böcker",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sidor",
                table: "Böcker");
        }
    }
}
