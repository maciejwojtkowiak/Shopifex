using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopifex.Migrations
{
    /// <inheritdoc />
    public partial class CartSave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSavedByUser",
                table: "Carts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSavedByUser",
                table: "Carts");
        }
    }
}
