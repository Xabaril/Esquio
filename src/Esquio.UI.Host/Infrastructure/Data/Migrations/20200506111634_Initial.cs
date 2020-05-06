using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Esquio.UI.Host.Infrastructure.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "History",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    FeatureName = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    NewValues = table.Column<string>(nullable: true),
                    OldValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: true),
                    FeatureName = table.Column<string>(nullable: true),
                    RingName = table.Column<string>(nullable: true),
                    Kind = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: false),
                    Kind = table.Column<string>(nullable: false, defaultValue: "User"),
                    ApplicationRole = table.Column<string>(nullable: false, defaultValue: "Reader")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    HexColor = table.Column<string>(maxLength: 7, nullable: true, defaultValue: "#FF0000")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deployments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    ByDefault = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    ProductEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deployments_Products_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    ProductEntityId = table.Column<int>(nullable: false),
                    Archived = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Features_Products_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureStates",
                schema: "dbo",
                columns: table => new
                {
                    FeatureEntityId = table.Column<int>(nullable: false),
                    DeploymentEntityId = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureStates", x => new { x.FeatureEntityId, x.DeploymentEntityId });
                    table.ForeignKey(
                        name: "FK_FeatureStates_Deployments_DeploymentEntityId",
                        column: x => x.DeploymentEntityId,
                        principalSchema: "dbo",
                        principalTable: "Deployments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureStates_Features_FeatureEntityId",
                        column: x => x.FeatureEntityId,
                        principalSchema: "dbo",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeatureTags",
                schema: "dbo",
                columns: table => new
                {
                    FeatureEntityId = table.Column<int>(nullable: false),
                    TagEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureTags", x => new { x.FeatureEntityId, x.TagEntityId });
                    table.ForeignKey(
                        name: "FK_FeatureTags_Features_FeatureEntityId",
                        column: x => x.FeatureEntityId,
                        principalSchema: "dbo",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureTags_Tags_TagEntityId",
                        column: x => x.TagEntityId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Toggles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    FeatureEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toggles", x => x.Id);
                    table.UniqueConstraint("IX_ToggeFeature", x => new { x.Type, x.FeatureEntityId });
                    table.ForeignKey(
                        name: "FK_Toggles_Features_FeatureEntityId",
                        column: x => x.FeatureEntityId,
                        principalSchema: "dbo",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToggleEntityId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Value = table.Column<string>(maxLength: 4000, nullable: false),
                    RingName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.UniqueConstraint("AK_Parameters_Name_RingName_ToggleEntityId", x => new { x.Name, x.RingName, x.ToggleEntityId });
                    table.ForeignKey(
                        name: "FK_Parameters_Toggles_ToggleEntityId",
                        column: x => x.ToggleEntityId,
                        principalSchema: "dbo",
                        principalTable: "Toggles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Key",
                schema: "dbo",
                table: "ApiKeys",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Name",
                schema: "dbo",
                table: "ApiKeys",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_ProductEntityId",
                schema: "dbo",
                table: "Deployments",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_ProductEntityId",
                schema: "dbo",
                table: "Features",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureStates_DeploymentEntityId",
                schema: "dbo",
                table: "FeatureStates",
                column: "DeploymentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureTags_TagEntityId",
                schema: "dbo",
                table: "FeatureTags",
                column: "TagEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_DateTime",
                schema: "dbo",
                table: "Metrics",
                column: "DateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_ToggleEntityId",
                schema: "dbo",
                table: "Parameters",
                column: "ToggleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SubjectId",
                schema: "dbo",
                table: "Permissions",
                column: "SubjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                schema: "dbo",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "dbo",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Toggles_FeatureEntityId",
                schema: "dbo",
                table: "Toggles",
                column: "FeatureEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FeatureStates",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FeatureTags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "History",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Metrics",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Parameters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Deployments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Toggles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Features",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");
        }
    }
}
