using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Magazalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SanalMi = table.Column<bool>(type: "bit", nullable: false),
                    Puani = table.Column<byte>(type: "tinyint", nullable: true),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrunMagazalar",
                columns: table => new
                {
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    MagazaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunMagazalar", x => new { x.UrunId, x.MagazaId });
                    table.ForeignKey(
                        name: "FK_UrunMagazalar_Magazalar_MagazaId",
                        column: x => x.MagazaId,
                        principalTable: "Magazalar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UrunMagazalar_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrunMagazalar_MagazaId",
                table: "UrunMagazalar",
                column: "MagazaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrunMagazalar");

            migrationBuilder.DropTable(
                name: "Magazalar");
        }
    }
}
