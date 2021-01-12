using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousWebApi.Migrations
{
    public partial class CountryId_AddedIn_StateMaster_made_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MasterState_CountryId",
                table: "MasterState",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterState_MasterCountry_CountryId",
                table: "MasterState",
                column: "CountryId",
                principalTable: "MasterCountry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterState_MasterCountry_CountryId",
                table: "MasterState");

            migrationBuilder.DropIndex(
                name: "IX_MasterState_CountryId",
                table: "MasterState");
        }
    }
}
