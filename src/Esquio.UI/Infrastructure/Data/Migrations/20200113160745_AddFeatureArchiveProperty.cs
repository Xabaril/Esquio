using Microsoft.EntityFrameworkCore.Migrations;

namespace Esquio.UI.Infrastructure.Data.Migrations
{
    public partial class AddFeatureArchiveProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Features",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Features");
        }
    }
}
