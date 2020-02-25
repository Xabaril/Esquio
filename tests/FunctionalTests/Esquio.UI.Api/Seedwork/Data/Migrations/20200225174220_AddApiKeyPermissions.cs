using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Data.Migrations
{
    public partial class AddApiKeyPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagementPermission",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ReadPermission",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "WritePermission",
                table: "Permissions");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Toggles",
                newName: "Toggles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tags",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Rings",
                newName: "Rings",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permissions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Parameters",
                newName: "Parameters",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "History",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FeatureTags",
                newName: "FeatureTags",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Features",
                newName: "Features",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ApiKeys",
                newName: "ApiKeys",
                newSchema: "dbo");

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

            migrationBuilder.RenameTable(
                name: "Toggles",
                schema: "dbo",
                newName: "Toggles");

            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "dbo",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Rings",
                schema: "dbo",
                newName: "Rings");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "dbo",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Permissions",
                schema: "dbo",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "Parameters",
                schema: "dbo",
                newName: "Parameters");

            migrationBuilder.RenameTable(
                name: "History",
                schema: "dbo",
                newName: "History");

            migrationBuilder.RenameTable(
                name: "FeatureTags",
                schema: "dbo",
                newName: "FeatureTags");

            migrationBuilder.RenameTable(
                name: "Features",
                schema: "dbo",
                newName: "Features");

            migrationBuilder.RenameTable(
                name: "ApiKeys",
                schema: "dbo",
                newName: "ApiKeys");

            migrationBuilder.AddColumn<bool>(
                name: "ManagementPermission",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReadPermission",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WritePermission",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
