using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropWord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumn_SentencesRepeatForDayModeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentencesRepeatForDayModeEnum",
                table: "UserSettings",
                newName: "SentencesRepeatForDayTimesModeEnum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentencesRepeatForDayTimesModeEnum",
                table: "UserSettings",
                newName: "SentencesRepeatForDayModeEnum");
        }
    }
}
