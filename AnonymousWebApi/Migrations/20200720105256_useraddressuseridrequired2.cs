using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousWebApi.Migrations
{
    public partial class useraddressuseridrequired2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserAddressInfo",
                nullable: false,
                defaultValue: "x",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserAddressInfo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValue: "x");
        }
    }
}
