using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Init_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Contacts_ContactId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Contacts_FriendContactId",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_FriendContactId",
                table: "Friends");

            migrationBuilder.AlterColumn<int>(
                name: "FriendContactId",
                table: "Friends",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Friends",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FriendUsername",
                table: "Friends",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                columns: new[] { "FriendContactId", "ContactId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Contacts_ContactId",
                table: "Friends",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Contacts_ContactId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "FriendUsername",
                table: "Friends");

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Friends",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FriendContactId",
                table: "Friends",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendContactId",
                table: "Friends",
                column: "FriendContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Contacts_ContactId",
                table: "Friends",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Contacts_FriendContactId",
                table: "Friends",
                column: "FriendContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
