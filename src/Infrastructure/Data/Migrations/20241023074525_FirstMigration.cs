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
                name: "AnalyticsUserAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TypeAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsUserAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutoChatBot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoChatBot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedNewFirstSentence",
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
                    table.PrimaryKey("PK_RecommendedNewFirstSentence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedNewSecondSentence",
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
                    table.PrimaryKey("PK_RecommendedNewSecondSentence", x => x.Id);
                });

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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedNewConnectionSentence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecommendedNewFirstSentenceId = table.Column<int>(type: "int", nullable: false),
                    RecommendedNewSecondSentenceId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedNewConnectionSentence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendedNewConnectionSentence_RecommendedNewFirstSentence_RecommendedNewFirstSentenceId",
                        column: x => x.RecommendedNewFirstSentenceId,
                        principalTable: "RecommendedNewFirstSentence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendedNewConnectionSentence_RecommendedNewSecondSentence_RecommendedNewSecondSentenceId",
                        column: x => x.RecommendedNewSecondSentenceId,
                        principalTable: "RecommendedNewSecondSentence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutoChatData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AutoChatBotId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoChatData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoChatData_AutoChatBot_AutoChatBotId",
                        column: x => x.AutoChatBotId,
                        principalTable: "AutoChatBot",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AutoChatData_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    SentencesRepeatForDayTimesModeEnum = table.Column<int>(type: "int", nullable: false),
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
                name: "RecommendedNewConnectionWithUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RecommendedNewConnectionSentenceId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedNewConnectionWithUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendedNewConnectionWithUser_RecommendedNewConnectionSentence_RecommendedNewConnectionSentenceId",
                        column: x => x.RecommendedNewConnectionSentenceId,
                        principalTable: "RecommendedNewConnectionSentence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendedNewConnectionWithUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutoChatHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderEnum = table.Column<int>(type: "int", nullable: false),
                    MessageTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoChatDataId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    WhenDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoChatHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoChatHistory_AutoChatData_AutoChatDataId",
                        column: x => x.AutoChatDataId,
                        principalTable: "AutoChatData",
                        principalColumn: "Id");
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
                name: "IX_AutoChatData_AutoChatBotId",
                table: "AutoChatData",
                column: "AutoChatBotId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoChatData_UserId",
                table: "AutoChatData",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoChatHistory_AutoChatDataId",
                table: "AutoChatHistory",
                column: "AutoChatDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedNewConnectionSentence_RecommendedNewFirstSentenceId",
                table: "RecommendedNewConnectionSentence",
                column: "RecommendedNewFirstSentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedNewConnectionSentence_RecommendedNewSecondSentenceId",
                table: "RecommendedNewConnectionSentence",
                column: "RecommendedNewSecondSentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedNewConnectionWithUser_RecommendedNewConnectionSentenceId",
                table: "RecommendedNewConnectionWithUser",
                column: "RecommendedNewConnectionSentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedNewConnectionWithUser_UserId",
                table: "RecommendedNewConnectionWithUser",
                column: "UserId");

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
                name: "AnalyticsUserAction");

            migrationBuilder.DropTable(
                name: "AutoChatHistory");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "RecommendedNewConnectionWithUser");

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
                name: "AutoChatData");

            migrationBuilder.DropTable(
                name: "RecommendedNewConnectionSentence");

            migrationBuilder.DropTable(
                name: "UserSentencesCollection");

            migrationBuilder.DropTable(
                name: "SentencesPair");

            migrationBuilder.DropTable(
                name: "AutoChatBot");

            migrationBuilder.DropTable(
                name: "RecommendedNewFirstSentence");

            migrationBuilder.DropTable(
                name: "RecommendedNewSecondSentence");

            migrationBuilder.DropTable(
                name: "Sentence");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
