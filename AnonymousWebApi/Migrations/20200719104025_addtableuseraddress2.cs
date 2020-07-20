using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousWebApi.Migrations
{
    public partial class addtableuseraddress2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "UserAddress",
                type: "nvarchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)");
        }
    }
}
