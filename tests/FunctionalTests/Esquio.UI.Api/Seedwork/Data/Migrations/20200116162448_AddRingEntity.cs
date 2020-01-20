using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Data.Migrations
{
    public partial class AddRingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Features",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Rings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    ProductEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rings_Products_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rings_ProductEntityId",
                table: "Rings",
                column: "ProductEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rings");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Features");
        }
    }
}
