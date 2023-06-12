using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class slotsfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotEntity_Users_UserId",
                table: "SlotEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlotEntity",
                table: "SlotEntity");

            migrationBuilder.RenameTable(
                name: "SlotEntity",
                newName: "Slots");

            migrationBuilder.RenameIndex(
                name: "IX_SlotEntity_UserId",
                table: "Slots",
                newName: "IX_Slots_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "SportObjectId",
                table: "Slots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slots",
                table: "Slots",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_SportObjectId",
                table: "Slots",
                column: "SportObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_SportObjects_SportObjectId",
                table: "Slots",
                column: "SportObjectId",
                principalTable: "SportObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Users_UserId",
                table: "Slots",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_SportObjects_SportObjectId",
                table: "Slots");

            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Users_UserId",
                table: "Slots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slots",
                table: "Slots");

            migrationBuilder.DropIndex(
                name: "IX_Slots_SportObjectId",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "SportObjectId",
                table: "Slots");

            migrationBuilder.RenameTable(
                name: "Slots",
                newName: "SlotEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Slots_UserId",
                table: "SlotEntity",
                newName: "IX_SlotEntity_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlotEntity",
                table: "SlotEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotEntity_Users_UserId",
                table: "SlotEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
