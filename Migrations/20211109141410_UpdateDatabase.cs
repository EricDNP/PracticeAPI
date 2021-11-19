using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PracticeAPI.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParentAs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profesion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentAs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParentBs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profesion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentBs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChildAs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentAId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profesion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildAs_ParentAs_ParentAId",
                        column: x => x.ParentAId,
                        principalTable: "ParentAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChildBs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentBId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profesion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildBs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildBs_ParentBs_ParentBId",
                        column: x => x.ParentBId,
                        principalTable: "ParentBs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildAs_ParentAId",
                table: "ChildAs",
                column: "ParentAId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildBs_ParentBId",
                table: "ChildBs",
                column: "ParentBId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildAs");

            migrationBuilder.DropTable(
                name: "ChildBs");

            migrationBuilder.DropTable(
                name: "ParentAs");

            migrationBuilder.DropTable(
                name: "ParentBs");
        }
    }
}
