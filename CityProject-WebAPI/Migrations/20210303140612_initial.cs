using Microsoft.EntityFrameworkCore.Migrations;

namespace CityProject_WebAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true),
                    Populacao = table.Column<int>(nullable: false),
                    CustoEstadoUS = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParametroCustos",
                columns: table => new
                {
                    PorPessoa = table.Column<double>(nullable: false),
                    ValorCorte = table.Column<int>(nullable: false),
                    Desconto = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true),
                    Populacao = table.Column<int>(nullable: false),
                    CustoCidadeUS = table.Column<double>(nullable: false),
                    EstadoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cidades_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "CustoEstadoUS", "Nome", "Populacao" },
                values: new object[] { 1, 16189204404.0, "Rio Grande do Sul", 1488252 });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "CustoEstadoUS", "Nome", "Populacao" },
                values: new object[] { 2, 5584959102.0, "Santa Catarina", 508826 });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "CustoEstadoUS", "Nome", "Populacao" },
                values: new object[] { 3, 6305506779.0, "Paraná", 575377 });

            migrationBuilder.InsertData(
                table: "Cidades",
                columns: new[] { "Id", "CustoCidadeUS", "EstadoId", "Nome", "Populacao" },
                values: new object[] { 1, 16189204404.0, 1, "Porto Alegre", 1488252 });

            migrationBuilder.InsertData(
                table: "Cidades",
                columns: new[] { "Id", "CustoCidadeUS", "EstadoId", "Nome", "Populacao" },
                values: new object[] { 2, 5584959102.0, 2, "Florianópolis", 508826 });

            migrationBuilder.InsertData(
                table: "Cidades",
                columns: new[] { "Id", "CustoCidadeUS", "EstadoId", "Nome", "Populacao" },
                values: new object[] { 3, 6305506779.0, 3, "Londrina", 575377 });

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades",
                column: "EstadoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "ParametroCustos");

            migrationBuilder.DropTable(
                name: "Estados");
        }
    }
}
