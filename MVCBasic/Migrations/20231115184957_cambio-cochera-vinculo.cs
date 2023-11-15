using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCBasic.Migrations
{
    /// <inheritdoc />
    public partial class cambiococheravinculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cocheras_Vehiculos_VehiculoId",
                table: "Cocheras");

            migrationBuilder.DropIndex(
                name: "IX_Cocheras_VehiculoId",
                table: "Cocheras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cocheras_VehiculoId",
                table: "Cocheras",
                column: "VehiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cocheras_Vehiculos_VehiculoId",
                table: "Cocheras",
                column: "VehiculoId",
                principalTable: "Vehiculos",
                principalColumn: "Id");
        }
    }
}
