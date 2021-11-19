using Microsoft.EntityFrameworkCore.Migrations;

namespace PracticeAPI.Migrations
{
    public partial class UpdateDatabase3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildAs_ParentAs_ParentAId",
                table: "ChildAs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildBs_ParentBs_ParentBId",
                table: "ChildBs");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildAs_ParentAs_ParentAId",
                table: "ChildAs",
                column: "ParentAId",
                principalTable: "ParentAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildBs_ParentBs_ParentBId",
                table: "ChildBs",
                column: "ParentBId",
                principalTable: "ParentBs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildAs_ParentAs_ParentAId",
                table: "ChildAs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildBs_ParentBs_ParentBId",
                table: "ChildBs");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildAs_ParentAs_ParentAId",
                table: "ChildAs",
                column: "ParentAId",
                principalTable: "ParentAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildBs_ParentBs_ParentBId",
                table: "ChildBs",
                column: "ParentBId",
                principalTable: "ParentBs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
