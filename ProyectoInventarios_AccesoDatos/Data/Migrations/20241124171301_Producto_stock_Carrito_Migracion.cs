using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoInventarios.Data.Migrations
{
    /// <inheritdoc />
    public partial class Producto_stock_Carrito_Migracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroSerie = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Costo = table.Column<double>(type: "float", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    MarcaId = table.Column<int>(type: "int", nullable: false),
                    PadreId = table.Column<int>(type: "int", nullable: true),
                    BodegaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Bodega_BodegaId",
                        column: x => x.BodegaId,
                        principalTable: "Bodega",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Produto_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Produto_Marca_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marca",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Produto_Produto_PadreId",
                        column: x => x.PadreId,
                        principalTable: "Produto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carrito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<int>(type: "int", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Municipio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime", nullable: false),
                    Confirmacion = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carrito_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carrito_Produto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockProducto_Produto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Produto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carrito_ProductoId",
                table: "Carrito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Carrito_UsuarioId",
                table: "Carrito",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_BodegaId",
                table: "Produto",
                column: "BodegaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_CategoriaId",
                table: "Produto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_MarcaId",
                table: "Produto",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_PadreId",
                table: "Produto",
                column: "PadreId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducto_ProductoId",
                table: "StockProducto",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carrito");

            migrationBuilder.DropTable(
                name: "StockProducto");

            migrationBuilder.DropTable(
                name: "Produto");
        }
    }
}
