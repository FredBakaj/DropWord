using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropWord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecommendedNewSentences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendedNewConnectionWithUser");

            migrationBuilder.DropTable(
                name: "RecommendedNewConnectionSentence");

            migrationBuilder.DropTable(
                name: "RecommendedNewFirstSentence");

            migrationBuilder.DropTable(
                name: "RecommendedNewSecondSentence");
        }
    }
}
