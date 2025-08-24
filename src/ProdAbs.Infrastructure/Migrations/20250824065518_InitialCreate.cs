using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProdAbs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoDeDocumentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageLocation = table.Column<string>(type: "text", nullable: false),
                    TamanhoEmBytes = table.Column<long>(type: "bigint", nullable: false),
                    HashTipo = table.Column<string>(type: "text", nullable: false),
                    HashValor = table.Column<string>(type: "text", nullable: false),
                    NomeArquivoOriginal = table.Column<string>(type: "text", nullable: false),
                    Formato = table.Column<string>(type: "text", nullable: false),
                    Versao = table.Column<int>(type: "integer", nullable: false),
                    DicionarioDeCamposValores = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prontuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorEntidade = table.Column<string>(type: "text", nullable: false),
                    TipoProntuario = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prontuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposDeDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDeDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampoMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false),
                    RegraDeValidacao_TipoDeDados = table.Column<int>(type: "integer", nullable: false),
                    RegraDeValidacao_Obrigatorio = table.Column<bool>(type: "boolean", nullable: false),
                    RegraDeValidacao_FormatoEspecifico = table.Column<string>(type: "text", nullable: false),
                    Mascara = table.Column<string>(type: "text", nullable: false),
                    TipoDocumentoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampoMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampoMetadata_TiposDeDocumento_TipoDocumentoId",
                        column: x => x.TipoDocumentoId,
                        principalTable: "TiposDeDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampoMetadata_TipoDocumentoId",
                table: "CampoMetadata",
                column: "TipoDocumentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampoMetadata");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Prontuarios");

            migrationBuilder.DropTable(
                name: "TiposDeDocumento");
        }
    }
}
