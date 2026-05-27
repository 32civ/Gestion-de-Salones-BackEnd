using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoPOO.Migrations
{
    /// <inheritdoc />
    public partial class FixRelacionHorario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_HorarioId",
                table: "Asignaciones",
                column: "HorarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignaciones_Horarios_HorarioId",
                table: "Asignaciones",
                column: "HorarioId",
                principalTable: "Horarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignaciones_Horarios_HorarioId",
                table: "Asignaciones");

            migrationBuilder.DropIndex(
                name: "IX_Asignaciones_HorarioId",
                table: "Asignaciones");
        }
    }
}
