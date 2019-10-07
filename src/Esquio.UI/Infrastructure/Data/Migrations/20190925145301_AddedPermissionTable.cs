using Microsoft.EntityFrameworkCore.Migrations;

namespace Esquio.UI.Infrastructure.Data.Migrations
{
    public partial class AddedPermissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: false),
                    ReadPermission = table.Column<bool>(nullable: false, defaultValue: false),
                    WritePermission = table.Column<bool>(nullable: false, defaultValue: false),
                    ManagementPermission = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SubjectId",
                table: "Permissions",
                column: "SubjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
