using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace independentia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFilmDescriptionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FilmPurchases",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FilmPurchases",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FilmPurchases");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FilmPurchases");
        }
    }
}
