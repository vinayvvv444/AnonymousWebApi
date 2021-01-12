using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousWebApi.Migrations
{
    public partial class addedtablemasterDistrictaddedcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MasterDistrict",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "MasterDistrict",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MasterDistrict",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "MasterDistrict",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MasterDistrict");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "MasterDistrict");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MasterDistrict");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "MasterDistrict");
        }
    }
}
