using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace independentia.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_hs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmPurchases_AspNetUsers_UserID",
                table: "FilmPurchases");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FilmPurchases");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "FilmPurchases",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FilmPurchases_UserID",
                table: "FilmPurchases",
                newName: "IX_FilmPurchases_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FilmPurchases",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmPurchases_AspNetUsers_UserId",
                table: "FilmPurchases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmPurchases_AspNetUsers_UserId",
                table: "FilmPurchases");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FilmPurchases",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_FilmPurchases_UserId",
                table: "FilmPurchases",
                newName: "IX_FilmPurchases_UserID");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FilmPurchases",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FilmPurchases",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmPurchases_AspNetUsers_UserID",
                table: "FilmPurchases",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
