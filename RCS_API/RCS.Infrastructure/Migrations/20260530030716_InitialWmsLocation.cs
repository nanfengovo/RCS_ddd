using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RCS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialWmsLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wms_locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CurrentTaskId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wms_locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_wms_locations_Code",
                table: "wms_locations",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wms_locations");
        }
    }
}
