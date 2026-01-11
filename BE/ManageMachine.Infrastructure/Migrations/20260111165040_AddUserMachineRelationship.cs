using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManageMachine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMachineRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Machines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_UserId",
                table: "Machines",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_UserId",
                table: "Machines",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_UserId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_UserId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Machines");
        }
    }
}
