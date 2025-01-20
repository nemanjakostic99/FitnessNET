using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnessNET.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendRequestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientProfileID",
                table: "ClientProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderID = table.Column<int>(type: "integer", nullable: false),
                    ReceiverID = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FriendRequests_ClientProfiles_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "ClientProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequests_ClientProfiles_SenderID",
                        column: x => x.SenderID,
                        principalTable: "ClientProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfiles_ClientProfileID",
                table: "ClientProfiles",
                column: "ClientProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverID",
                table: "FriendRequests",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_SenderID",
                table: "FriendRequests",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProfiles_ClientProfiles_ClientProfileID",
                table: "ClientProfiles",
                column: "ClientProfileID",
                principalTable: "ClientProfiles",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProfiles_ClientProfiles_ClientProfileID",
                table: "ClientProfiles");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropIndex(
                name: "IX_ClientProfiles_ClientProfileID",
                table: "ClientProfiles");

            migrationBuilder.DropColumn(
                name: "ClientProfileID",
                table: "ClientProfiles");
        }
    }
}
