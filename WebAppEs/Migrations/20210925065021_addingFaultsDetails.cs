using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppEs.Migrations
{
    public partial class addingFaultsDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MobileRNDFaultDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeID = table.Column<string>(maxLength: 50, nullable: false),
                    MobileRNDFaultsEntryId = table.Column<Guid>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(maxLength: 50, nullable: false),
                    FaultType = table.Column<string>(maxLength: 250, nullable: false),
                    FaultQty = table.Column<int>(maxLength: 50, nullable: false),
                    RootCause = table.Column<string>(nullable: true),
                    Solution = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileRNDFaultDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileRNDFaultDetails_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MobileRNDFaultDetails_MobileRNDFaultsEntry_MobileRNDFaultsEntryId",
                        column: x => x.MobileRNDFaultsEntryId,
                        principalTable: "MobileRNDFaultsEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MobileRNDFaultDetails_ApplicationUserId",
                table: "MobileRNDFaultDetails",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileRNDFaultDetails_MobileRNDFaultsEntryId",
                table: "MobileRNDFaultDetails",
                column: "MobileRNDFaultsEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileRNDFaultDetails");
        }
    }
}
