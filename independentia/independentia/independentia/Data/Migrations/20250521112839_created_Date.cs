using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace independentia.Data.Migrations
{
    /// <inheritdoc />
    public partial class created_Date : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Films",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Films");
        }
    }
}
