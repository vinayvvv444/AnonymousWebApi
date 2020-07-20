using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousWebApi.Migrations
{
    public partial class addtableuseraddress3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "District",
                table: "UserAddress",
                newName: "DistrictOrCity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DistrictOrCity",
                table: "UserAddress",
                newName: "District");
        }
    }
}
