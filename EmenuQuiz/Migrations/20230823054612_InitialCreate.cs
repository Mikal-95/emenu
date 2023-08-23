using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EmenuQuiz.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", precision: 10, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id);
                })
                .Annotation("Relational:Collation", "USING_NLS_COMP");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", precision: 10, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    inventory_number = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false),
                    cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id);
                })
                .Annotation("Relational:Collation", "USING_NLS_COMP");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", precision: 10, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    public_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    image_url = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    image_size = table.Column<long>(type: "bigint", precision: 10, nullable: false),
                    product_id = table.Column<int>(type: "int", precision: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.id);
                    table.ForeignKey(
                        name: "Image_FK",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id");
                })
                .Annotation("Relational:Collation", "USING_NLS_COMP");

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", precision: 10, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    product_id = table.Column<int>(type: "int", precision: 10, nullable: false),
                    category_id = table.Column<int>(type: "int", precision: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.id);
                    table.ForeignKey(
                        name: "ProductCategory_FK",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ProductCategory_FK_1",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "USING_NLS_COMP");

            migrationBuilder.CreateTable(
                name: "ProductTranslation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", precision: 10, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    lang = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    value = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    product_id = table.Column<int>(type: "int", precision: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTranslation", x => x.id);
                    table.ForeignKey(
                        name: "ProductTranslation_FK",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "USING_NLS_COMP");

            migrationBuilder.CreateIndex(
                name: "IX_Image_product_id",
                table: "Image",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_category_id",
                table: "ProductCategory",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_product_id",
                table: "ProductCategory",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslation_product_id",
                table: "ProductTranslation",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "ProductTranslation");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
