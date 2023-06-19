using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.DAL.Migrations
{
    /// <inheritdoc />
    public partial class extraInfoInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "Contacts",
                table: "AspNetUsers",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Myself",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Sports",
                table: "AspNetUsers",
                type: "integer[]",
                nullable: true,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contacts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Myself",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sports",
                table: "AspNetUsers");
        }
    }
}
