using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hubtel.Wallet.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "wallets",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "wallets",
                newName: "name");
        }
    }
}
