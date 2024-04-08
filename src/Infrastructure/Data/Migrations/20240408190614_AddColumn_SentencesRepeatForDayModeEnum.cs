using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropWord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn_SentencesRepeatForDayModeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SentencesRepeatForDayModeEnum",
                table: "UserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentencesRepeatForDayModeEnum",
                table: "UserSettings");
        }
    }
}
