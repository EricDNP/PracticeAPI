using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PracticeAPI.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThingAs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildAId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Num = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThingAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThingAs_ChildAs_ChildAId",
                        column: x => x.ChildAId,
                        principalTable: "ChildAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThingBs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildBId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Num = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThingBs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThingBs_ChildBs_ChildBId",
                        column: x => x.ChildBId,
                        principalTable: "ChildBs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThingAs_ChildAId",
                table: "ThingAs",
                column: "ChildAId");

            migrationBuilder.CreateIndex(
                name: "IX_ThingBs_ChildBId",
                table: "ThingBs",
                column: "ChildBId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThingAs");

            migrationBuilder.DropTable(
                name: "ThingBs");
        }
    }
}
