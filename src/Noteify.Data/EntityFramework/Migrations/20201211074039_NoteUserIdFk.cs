using Microsoft.EntityFrameworkCore.Migrations;

namespace Noteify.Data.EntityFramework.Migrations
{
    public partial class NoteUserIdFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Note",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_UserId",
                table: "Note",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_User_UserId",
                table: "Note",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_User_UserId",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_UserId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Note");
        }
    }
}
