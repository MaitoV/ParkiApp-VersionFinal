using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCBasic.Migrations
{
    /// <inheritdoc />
    public partial class agregartipovehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoVehiculo",
                table: "Cocheras",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoVehiculo",
                table: "Cocheras");
        }
    }
}
