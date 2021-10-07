using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppEs.Migrations
{
    public partial class updateEntityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileRNDFaultsEntry_AspNetUsers_ApplicationUserId",
                table: "MobileRNDFaultsEntry");

            migrationBuilder.DropIndex(
                name: "IX_MobileRNDFaultsEntry_ApplicationUserId",
                table: "MobileRNDFaultsEntry");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "MobileRNDFaultsEntry");

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "MobileRNDFaultsEntry",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "MobileRNDFaultsEntry");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "MobileRNDFaultsEntry",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileRNDFaultsEntry_ApplicationUserId",
                table: "MobileRNDFaultsEntry",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileRNDFaultsEntry_AspNetUsers_ApplicationUserId",
                table: "MobileRNDFaultsEntry",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
