using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UntitledArticles.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeParentCategoryIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Id_ParentCategoryId",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Id_ParentCategoryId",
                table: "Categories",
                columns: new[] { "Id", "ParentCategoryId" })
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
