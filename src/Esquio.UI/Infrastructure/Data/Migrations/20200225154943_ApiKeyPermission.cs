using Microsoft.EntityFrameworkCore.Migrations;

namespace Esquio.UI.Infrastructure.Data.Migrations
{
    public partial class ApiKeyPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagementPermission",
                schema: "dbo",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ReadPermission",
                schema: "dbo",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "WritePermission",
                schema: "dbo",
                table: "Permissions");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationRole",
                schema: "dbo",
                table: "Permissions",
                nullable: false,
                defaultValue: "Reader");

            migrationBuilder.AddColumn<string>(
                name: "Kind",
                schema: "dbo",
                table: "Permissions",
                nullable: false,
                defaultValue: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationRole",
                schema: "dbo",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Kind",
                schema: "dbo",
                table: "Permissions");

            migrationBuilder.AddColumn<bool>(
                name: "ManagementPermission",
                schema: "dbo",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReadPermission",
                schema: "dbo",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WritePermission",
                schema: "dbo",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
