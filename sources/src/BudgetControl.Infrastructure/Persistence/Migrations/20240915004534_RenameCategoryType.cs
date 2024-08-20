using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetControl.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameCategoryType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Categories",
                newName: "CategoryType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryType",
                table: "Categories",
                newName: "Type");
        }
    }
}
