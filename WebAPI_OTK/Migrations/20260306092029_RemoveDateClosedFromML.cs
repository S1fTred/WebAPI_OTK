using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_OTK.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDateClosedFromML : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ДатаЗакрытия",
                table: "МЛ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ДатаЗакрытия",
                table: "МЛ",
                type: "date",
                nullable: true);
        }
    }
}
