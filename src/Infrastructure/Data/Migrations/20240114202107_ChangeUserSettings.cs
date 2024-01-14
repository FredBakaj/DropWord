using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropWord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondLanguage",
                table: "UserSettings",
                newName: "MainLanguage");

            migrationBuilder.RenameColumn(
                name: "FirstLanguage",
                table: "UserSettings",
                newName: "LearnLanguage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainLanguage",
                table: "UserSettings",
                newName: "SecondLanguage");

            migrationBuilder.RenameColumn(
                name: "LearnLanguage",
                table: "UserSettings",
                newName: "FirstLanguage");
        }
    }
}
