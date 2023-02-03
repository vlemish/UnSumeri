using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UntitledSelfArticles.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WithCategoryMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    NodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.NodeId);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryNodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryNodeId",
                        column: x => x.CategoryNodeId,
                        principalTable: "Categories",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AncestoryCategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryMappings_Categories_AncestoryCategoryId",
                        column: x => x.AncestoryCategoryId,
                        principalTable: "Categories",
                        principalColumn: "NodeId");
                    table.ForeignKey(
                        name: "FK_CategoryMappings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryNodeId",
                table: "Articles",
                column: "CategoryNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMappings_AncestoryCategoryId",
                table: "CategoryMappings",
                column: "AncestoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMappings_CategoryId",
                table: "CategoryMappings",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "CategoryMappings");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
