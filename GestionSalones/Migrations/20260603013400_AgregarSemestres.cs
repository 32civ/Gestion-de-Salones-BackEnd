using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionSalones.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSemestres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SemestreId",
                table: "Matriculas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SemestreId",
                table: "Cursos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Semestres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semestres", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_EstudianteId_CursoId_SemestreId",
                table: "Matriculas",
                columns: new[] { "EstudianteId", "CursoId", "SemestreId" },
                unique: true,
                filter: "[SemestreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_SemestreId",
                table: "Matriculas",
                column: "SemestreId");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_SemestreId",
                table: "Cursos",
                column: "SemestreId");

            migrationBuilder.CreateIndex(
                name: "IX_Semestres_Nombre",
                table: "Semestres",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cursos_Semestres_SemestreId",
                table: "Cursos",
                column: "SemestreId",
                principalTable: "Semestres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Semestres_SemestreId",
                table: "Matriculas",
                column: "SemestreId",
                principalTable: "Semestres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cursos_Semestres_SemestreId",
                table: "Cursos");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Semestres_SemestreId",
                table: "Matriculas");

            migrationBuilder.DropTable(
                name: "Semestres");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_EstudianteId_CursoId_SemestreId",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_SemestreId",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Cursos_SemestreId",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "SemestreId",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "SemestreId",
                table: "Cursos");
        }
    }
}
