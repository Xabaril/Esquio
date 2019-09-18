using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Esquio.UI.Infrastructure.Data.Migrations
{
    public partial class AddedValidTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Toggles_ToggleEntityId1",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_ToggleEntityId1",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ToggleEntityId1",
                table: "Parameters");

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

            migrationBuilder.AddColumn<int>(
                name: "ToggleEntityId1",
                table: "Parameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApiKeys",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_ToggleEntityId1",
                table: "Parameters",
                column: "ToggleEntityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Toggles_ToggleEntityId1",
                table: "Parameters",
                column: "ToggleEntityId1",
                principalTable: "Toggles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
