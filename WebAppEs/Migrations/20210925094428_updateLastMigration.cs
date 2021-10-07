using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppEs.Migrations
{
    public partial class updateLastMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileRNDFaultDetails_AspNetUsers_ApplicationUserId",
                table: "MobileRNDFaultDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MobileRNDFaultDetails_MobileRNDFaultsEntry_MobileRNDFaultsEntryId",
                table: "MobileRNDFaultDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileRNDFaultDetails_ApplicationUserId",
                table: "MobileRNDFaultDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileRNDFaultDetails_MobileRNDFaultsEntryId",
                table: "MobileRNDFaultDetails");

            migrationBuilder.DropColumn(
                name: "MobileRNDFaultsEntryId",
                table: "MobileRNDFaultDetails");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "MobileRNDFaultDetails",
                newName: "ApplicationUserID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserID",
                table: "MobileRNDFaultDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FaultEntryID",
                table: "MobileRNDFaultDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MobileRNDFaultDetails_FaultEntryID",
                table: "MobileRNDFaultDetails",
                column: "FaultEntryID");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileRNDFaultDetails_MobileRNDFaultsEntry_FaultEntryID",
                table: "MobileRNDFaultDetails",
                column: "FaultEntryID",
                principalTable: "MobileRNDFaultsEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileRNDFaultDetails_MobileRNDFaultsEntry_FaultEntryID",
                table: "MobileRNDFaultDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileRNDFaultDetails_FaultEntryID",
                table: "MobileRNDFaultDetails");

            migrationBuilder.DropColumn(
                name: "FaultEntryID",
                table: "MobileRNDFaultDetails");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "MobileRNDFaultDetails",
                newName: "ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "MobileRNDFaultDetails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "MobileRNDFaultsEntryId",
                table: "MobileRNDFaultDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileRNDFaultDetails_ApplicationUserId",
                table: "MobileRNDFaultDetails",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileRNDFaultDetails_MobileRNDFaultsEntryId",
                table: "MobileRNDFaultDetails",
                column: "MobileRNDFaultsEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileRNDFaultDetails_AspNetUsers_ApplicationUserId",
                table: "MobileRNDFaultDetails",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MobileRNDFaultDetails_MobileRNDFaultsEntry_MobileRNDFaultsEntryId",
                table: "MobileRNDFaultDetails",
                column: "MobileRNDFaultsEntryId",
                principalTable: "MobileRNDFaultsEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
