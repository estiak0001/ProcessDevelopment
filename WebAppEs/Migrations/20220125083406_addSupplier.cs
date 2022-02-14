using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppEs.Migrations
{
    public partial class addSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "MobileRNDPartsModels");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "MobileRNDPartsModels",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MobileRNDSupplier",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplierName = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileRNDSupplier", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileRNDSupplier");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "MobileRNDPartsModels");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "MobileRNDPartsModels",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
