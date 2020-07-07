using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Esquio.UI.Store.Infrastructure.Data.NpgSql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductName = table.Column<string>(nullable: true),
                    FeatureName = table.Column<string>(nullable: true),
                    DeploymentName = table.Column<string>(nullable: true),
                    Kind = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    HexColor = table.Column<string>(maxLength: 7, nullable: true, defaultValue: "#FF0000")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deployments",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    ByDefault = table.Column<bool>(nullable: false, defaultValueSql: "false"),
                    ProductEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deployments_Products_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureStates",
                schema: "public",
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
                        principalSchema: "public",
                        principalTable: "Deployments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureStates_Features_FeatureEntityId",
                        column: x => x.FeatureEntityId,
                        principalSchema: "public",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeatureTags",
                schema: "public",
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
                        principalSchema: "public",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureTags_Tags_TagEntityId",
                        column: x => x.TagEntityId,
                        principalSchema: "public",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Toggles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        principalSchema: "public",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ToggleEntityId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Value = table.Column<string>(maxLength: 4000, nullable: false),
                    DeploymentName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.UniqueConstraint("AK_Parameters_Name_DeploymentName_ToggleEntityId", x => new { x.Name, x.DeploymentName, x.ToggleEntityId });
                    table.ForeignKey(
                        name: "FK_Parameters_Toggles_ToggleEntityId",
                        column: x => x.ToggleEntityId,
                        principalSchema: "public",
                        principalTable: "Toggles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Key",
                schema: "public",
                table: "ApiKeys",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Name",
                schema: "public",
                table: "ApiKeys",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_ProductEntityId",
                schema: "public",
                table: "Deployments",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_ProductEntityId",
                schema: "public",
                table: "Features",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureStates_DeploymentEntityId",
                schema: "public",
                table: "FeatureStates",
                column: "DeploymentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureTags_TagEntityId",
                schema: "public",
                table: "FeatureTags",
                column: "TagEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_DateTime",
                schema: "public",
                table: "Metrics",
                column: "DateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_ToggleEntityId",
                schema: "public",
                table: "Parameters",
                column: "ToggleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SubjectId",
                schema: "public",
                table: "Permissions",
                column: "SubjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                schema: "public",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "public",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Toggles_FeatureEntityId",
                schema: "public",
                table: "Toggles",
                column: "FeatureEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FeatureStates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FeatureTags",
                schema: "public");

            migrationBuilder.DropTable(
                name: "History",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Metrics",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Parameters",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Deployments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Toggles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Features",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");
        }
    }
}
