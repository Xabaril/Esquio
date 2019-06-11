using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Esquio.UI.API.Seedwork.Data.Migrations
{
    public partial class AddClrTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClrType",
                table: "Parameters",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClrType",
                table: "Parameters");
        }
    }
}
