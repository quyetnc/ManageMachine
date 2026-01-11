using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManageMachine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQRCodeDataFromMachine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRCodeData",
                table: "Machines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QRCodeData",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
