using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb2Databaser.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Butiker",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Butik = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Adress = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Butiker__3214EC2736F840B5", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Författare",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Förnamn = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Efternamn = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Födelsedatum = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Författa__3214EC2769723AD1", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kunder",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Förnamn = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Efternam = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kunder__3214EC27728FC7B0", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Böcker",
                columns: table => new
                {
                    ISBN13 = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    FörfattarID = table.Column<int>(type: "int", nullable: true),
                    Titel = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Språk = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Pris = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Böcker__3BF79E039F4E5330", x => x.ISBN13);
                    table.ForeignKey(
                        name: "FK__Böcker__Författa__4222D4EF",
                        column: x => x.FörfattarID,
                        principalTable: "Författare",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Ordrar",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    KundID = table.Column<int>(type: "int", nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: true),
                    StyckPris = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ordrar__C3905BAF9C561485", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK__Ordrar__KundID__30F848ED",
                        column: x => x.KundID,
                        principalTable: "Kunder",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Förlag",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN13 = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Företag = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Förlag__3214EC271A558003", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Förlag__ISBN13__5CD6CB2B",
                        column: x => x.ISBN13,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13");
                });

            migrationBuilder.CreateTable(
                name: "LagerSaldo",
                columns: table => new
                {
                    ButikID = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LagerSal__1191B8942DB7830D", x => new { x.ButikID, x.ISBN });
                    table.ForeignKey(
                        name: "FK__LagerSald__Butik__46E78A0C",
                        column: x => x.ButikID,
                        principalTable: "Butiker",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__LagerSaldo__ISBN__47DBAE45",
                        column: x => x.ISBN,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Böcker_FörfattarID",
                table: "Böcker",
                column: "FörfattarID");

            migrationBuilder.CreateIndex(
                name: "IX_Förlag_ISBN13",
                table: "Förlag",
                column: "ISBN13");

            migrationBuilder.CreateIndex(
                name: "IX_LagerSaldo_ISBN",
                table: "LagerSaldo",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Ordrar_KundID",
                table: "Ordrar",
                column: "KundID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Förlag");

            migrationBuilder.DropTable(
                name: "LagerSaldo");

            migrationBuilder.DropTable(
                name: "Ordrar");

            migrationBuilder.DropTable(
                name: "Butiker");

            migrationBuilder.DropTable(
                name: "Böcker");

            migrationBuilder.DropTable(
                name: "Kunder");

            migrationBuilder.DropTable(
                name: "Författare");
        }
    }
}
