using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Data.Migrations
{
    public partial class RemoveInvalidParametersUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parameters_Name",
                table: "Parameters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Parameters_Name",
                table: "Parameters",
                column: "Name",
                unique: true);
        }
    }
}
