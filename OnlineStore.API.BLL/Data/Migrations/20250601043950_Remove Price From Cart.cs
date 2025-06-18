using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.API.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceFromCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "CartItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
