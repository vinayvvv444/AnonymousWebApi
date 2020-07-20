using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousWebApi.Migrations
{
    public partial class addtableuseraddress4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_Users_UserId",
                table: "UserAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress");

            migrationBuilder.RenameTable(
                name: "UserAddress",
                newName: "UserAddressInfo");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddress_UserId",
                table: "UserAddressInfo",
                newName: "IX_UserAddressInfo_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddressInfo",
                table: "UserAddressInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddressInfo_Users_UserId",
                table: "UserAddressInfo",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddressInfo_Users_UserId",
                table: "UserAddressInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddressInfo",
                table: "UserAddressInfo");

            migrationBuilder.RenameTable(
                name: "UserAddressInfo",
                newName: "UserAddress");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddressInfo_UserId",
                table: "UserAddress",
                newName: "IX_UserAddress_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_Users_UserId",
                table: "UserAddress",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
