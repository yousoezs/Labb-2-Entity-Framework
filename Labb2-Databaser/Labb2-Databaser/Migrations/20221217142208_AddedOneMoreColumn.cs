using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb2Databaser.Migrations
{
    /// <inheritdoc />
    public partial class AddedOneMoreColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UtgivningsDatum",
                table: "Böcker",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtgivningsDatum",
                table: "Böcker");
        }
    }
}
