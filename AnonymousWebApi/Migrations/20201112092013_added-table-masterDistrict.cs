using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousWebApi.Migrations
{
    public partial class addedtablemasterDistrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterDistrict",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDistrict", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterDistrict_MasterCountry_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MasterCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterDistrict_MasterState_StateId",
                        column: x => x.StateId,
                        principalTable: "MasterState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDistrict_CountryId",
                table: "MasterDistrict",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDistrict_StateId",
                table: "MasterDistrict",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterDistrict");
        }
    }
}
