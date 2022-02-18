using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class RenameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_packs_packs_pack_id",
                table: "users_packs");

            migrationBuilder.DropForeignKey(
                name: "fk_users_packs_users_user_id",
                table: "users_packs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users_packs",
                table: "users_packs");

            migrationBuilder.RenameTable(
                name: "users_packs",
                newName: "user_packs");

            migrationBuilder.RenameIndex(
                name: "ix_users_packs_user_id",
                table: "user_packs",
                newName: "ix_user_packs_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_users_packs_pack_id",
                table: "user_packs",
                newName: "ix_user_packs_pack_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_packs",
                table: "user_packs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_packs_packs_pack_id",
                table: "user_packs",
                column: "pack_id",
                principalTable: "packs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_packs_users_user_id",
                table: "user_packs",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_packs_packs_pack_id",
                table: "user_packs");

            migrationBuilder.DropForeignKey(
                name: "fk_user_packs_users_user_id",
                table: "user_packs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_packs",
                table: "user_packs");

            migrationBuilder.RenameTable(
                name: "user_packs",
                newName: "users_packs");

            migrationBuilder.RenameIndex(
                name: "ix_user_packs_user_id",
                table: "users_packs",
                newName: "ix_users_packs_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_packs_pack_id",
                table: "users_packs",
                newName: "ix_users_packs_pack_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users_packs",
                table: "users_packs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_packs_packs_pack_id",
                table: "users_packs",
                column: "pack_id",
                principalTable: "packs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_users_packs_users_user_id",
                table: "users_packs",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
