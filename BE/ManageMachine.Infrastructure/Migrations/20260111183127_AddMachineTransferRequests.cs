using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManageMachine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineTransferRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_UserId",
                table: "Machines");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Machines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Machines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MachineTransferRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    FromUserId = table.Column<int>(type: "int", nullable: false),
                    ToUserId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTransferRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineTransferRequests_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachineTransferRequests_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineTransferRequests_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Machines_TenantId",
                table: "Machines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTransferRequests_FromUserId",
                table: "MachineTransferRequests",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTransferRequests_MachineId",
                table: "MachineTransferRequests",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTransferRequests_ToUserId",
                table: "MachineTransferRequests",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_TenantId",
                table: "Machines",
                column: "TenantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_UserId",
                table: "Machines",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_TenantId",
                table: "Machines");

            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_UserId",
                table: "Machines");

            migrationBuilder.DropTable(
                name: "MachineTransferRequests");

            migrationBuilder.DropIndex(
                name: "IX_Machines_TenantId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Machines");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_UserId",
                table: "Machines",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
