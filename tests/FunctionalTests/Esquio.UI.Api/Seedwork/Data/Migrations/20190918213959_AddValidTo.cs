using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Data.Migrations
{
    public partial class AddValidTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ApiKeys");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "ApiKeys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "ApiKeys");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApiKeys",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
