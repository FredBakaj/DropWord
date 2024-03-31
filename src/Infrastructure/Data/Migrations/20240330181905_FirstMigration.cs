using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropWord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sentence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SentencesPair",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    FirstSentenceId = table.Column<int>(type: "int", nullable: false),
                    SecondSentenceId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentencesPair", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SentencesPair_Sentence_FirstSentenceId",
                        column: x => x.FirstSentenceId,
                        principalTable: "Sentence",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SentencesPair_Sentence_SecondSentenceId",
                        column: x => x.SecondSentenceId,
                        principalTable: "Sentence",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SentencesPair_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StateTree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonTempData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateTree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateTree_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLearningInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUseForDaySentencesId = table.Column<int>(type: "int", nullable: true),
                    CountUseForDaySentences = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLearningInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLearningInfo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSentencesCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSentencesCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSentencesCollection_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterfaceLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearnSentencesModeEnum = table.Column<int>(type: "int", nullable: false),
                    MainLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearnLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentencesRepeatForDayModeEnum = table.Column<int>(type: "int", nullable: false),
                    TimeZone = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsingSentencesPair",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLearning = table.Column<bool>(type: "bit", nullable: false),
                    CountUse = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SentencesPairId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsingSentencesPair", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsingSentencesPair_SentencesPair_SentencesPairId",
                        column: x => x.SentencesPairId,
                        principalTable: "SentencesPair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsingSentencesPair_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SentencesPairEntityUserSentencesCollectionEntity",
                columns: table => new
                {
                    SentencesPairsId = table.Column<int>(type: "int", nullable: false),
                    UserSentencesCollectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentencesPairEntityUserSentencesCollectionEntity", x => new { x.SentencesPairsId, x.UserSentencesCollectionsId });
                    table.ForeignKey(
                        name: "FK_SentencesPairEntityUserSentencesCollectionEntity_SentencesPair_SentencesPairsId",
                        column: x => x.SentencesPairsId,
                        principalTable: "SentencesPair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SentencesPairEntityUserSentencesCollectionEntity_UserSentencesCollection_UserSentencesCollectionsId",
                        column: x => x.UserSentencesCollectionsId,
                        principalTable: "UserSentencesCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SentencesPair_FirstSentenceId",
                table: "SentencesPair",
                column: "FirstSentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SentencesPair_SecondSentenceId",
                table: "SentencesPair",
                column: "SecondSentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SentencesPair_UserId",
                table: "SentencesPair",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SentencesPairEntityUserSentencesCollectionEntity_UserSentencesCollectionsId",
                table: "SentencesPairEntityUserSentencesCollectionEntity",
                column: "UserSentencesCollectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_StateTree_UserId",
                table: "StateTree",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLearningInfo_UserId",
                table: "UserLearningInfo",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSentencesCollection_UserId",
                table: "UserSentencesCollection",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsingSentencesPair_SentencesPairId",
                table: "UsingSentencesPair",
                column: "SentencesPairId");

            migrationBuilder.CreateIndex(
                name: "IX_UsingSentencesPair_UserId",
                table: "UsingSentencesPair",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SentencesPairEntityUserSentencesCollectionEntity");

            migrationBuilder.DropTable(
                name: "StateTree");

            migrationBuilder.DropTable(
                name: "UserLearningInfo");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "UsingSentencesPair");

            migrationBuilder.DropTable(
                name: "UserSentencesCollection");

            migrationBuilder.DropTable(
                name: "SentencesPair");

            migrationBuilder.DropTable(
                name: "Sentence");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
