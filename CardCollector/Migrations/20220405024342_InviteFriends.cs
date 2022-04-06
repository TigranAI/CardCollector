using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class InviteFriends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "invite_infoid",
                table: "users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "invite_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    invite_key = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    invited_by_id = table.Column<long>(type: "bigint", nullable: true),
                    invited_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invite_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_invite_info_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invite_info_users_invited_by_id",
                        column: x => x.invited_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_users_invite_infoid",
                table: "users",
                column: "invite_infoid");

            migrationBuilder.CreateIndex(
                name: "ix_invite_info_invited_by_id",
                table: "invite_info",
                column: "invited_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_invite_info_invite_infoid",
                table: "users",
                column: "invite_infoid",
                principalTable: "invite_info",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_invite_info_invite_infoid",
                table: "users");

            migrationBuilder.DropTable(
                name: "invite_info");

            migrationBuilder.DropIndex(
                name: "ix_users_invite_infoid",
                table: "users");

            migrationBuilder.DropColumn(
                name: "invite_infoid",
                table: "users");
        }
    }
}
