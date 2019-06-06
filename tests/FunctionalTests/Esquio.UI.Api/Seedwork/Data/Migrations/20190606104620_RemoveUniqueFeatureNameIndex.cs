using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Esquio.UI.API.Seedwork.Data.Migrations
{
    public partial class RemoveUniqueFeatureNameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Features_Name",
                table: "Features");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Features_Name",
                table: "Features",
                column: "Name",
                unique: true);
        }
    }
}
