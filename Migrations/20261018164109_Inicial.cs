using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VisualSync.Api.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaquetesServicio",
                columns: table => new
                {
                    IdPaquete = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Duracion = table.Column<int>(type: "integer", nullable: false),
                    CantidadFotos = table.Column<int>(type: "integer", nullable: false),
                    IncluyeVideo = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaquetesServicio", x => x.IdPaquete);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdPaquete = table.Column<int>(type: "integer", nullable: false),
                    FechaEvento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LugarEvento = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EstadoReserva = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EstadoPago = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RutaComprobanteQR = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    MotivoRechazo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DescargasHabilitadas = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_Reservas_PaquetesServicio_IdPaquete",
                        column: x => x.IdPaquete,
                        principalTable: "PaquetesServicio",
                        principalColumn: "IdPaquete",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArchivosGaleria",
                columns: table => new
                {
                    IdArchivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdReserva = table.Column<int>(type: "integer", nullable: false),
                    RutaBajaRes = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    RutaAltaRes = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    TipoArchivo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SeleccionadaPorCliente = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosGaleria", x => x.IdArchivo);
                    table.ForeignKey(
                        name: "FK_ArchivosGaleria_Reservas_IdReserva",
                        column: x => x.IdReserva,
                        principalTable: "Reservas",
                        principalColumn: "IdReserva",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosGaleria_IdReserva",
                table: "ArchivosGaleria",
                column: "IdReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdPaquete",
                table: "Reservas",
                column: "IdPaquete");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdUsuario",
                table: "Reservas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivosGaleria");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "PaquetesServicio");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
