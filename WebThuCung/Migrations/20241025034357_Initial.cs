﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebThuCung.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    idAdmin = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    userAdmin = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    passwordAdmin = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.idAdmin);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    idBranch = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameBranch = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.idBranch);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    idCategory = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameCategory = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.idCategory);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    idColor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameColor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.idColor);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    idCountry = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.idCountry);
                });

            migrationBuilder.CreateTable(
                name: "Mission",
                columns: table => new
                {
                    idMission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    nameMission = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mission", x => x.idMission);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    idPet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    namePet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.idPet);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    idSize = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameSize = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.idSize);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    idSupplier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameSupplier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.idSupplier);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    idCity = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idCountry = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.idCity);
                    table.ForeignKey(
                        name: "FK_City_Country_idCountry",
                        column: x => x.idCountry,
                        principalTable: "Country",
                        principalColumn: "idCountry",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    idAdmin = table.Column<int>(type: "int", nullable: false),
                    idRole = table.Column<int>(type: "int", nullable: false),
                    idMission = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AdminidAdmin = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => new { x.idAdmin, x.idRole });
                    table.ForeignKey(
                        name: "FK_Role_Admin_AdminidAdmin",
                        column: x => x.AdminidAdmin,
                        principalTable: "Admin",
                        principalColumn: "idAdmin");
                    table.ForeignKey(
                        name: "FK_Role_Mission_idMission",
                        column: x => x.idMission,
                        principalTable: "Mission",
                        principalColumn: "idMission",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameProduct = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    sellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    idBranch = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idCategory = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idPet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.idProduct);
                    table.ForeignKey(
                        name: "FK_Product_Branch_idBranch",
                        column: x => x.idBranch,
                        principalTable: "Branch",
                        principalColumn: "idBranch",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Category_idCategory",
                        column: x => x.idCategory,
                        principalTable: "Category",
                        principalColumn: "idCategory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Pet_idPet",
                        column: x => x.idPet,
                        principalTable: "Pet",
                        principalColumn: "idPet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoteWarehouse",
                columns: table => new
                {
                    idVotewarehouse = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    dateEntry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idSupplier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    totalVoteWarehouse = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteWarehouse", x => x.idVotewarehouse);
                    table.ForeignKey(
                        name: "FK_VoteWarehouse_Supplier_idSupplier",
                        column: x => x.idSupplier,
                        principalTable: "Supplier",
                        principalColumn: "idSupplier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    idDistrict = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameDistrict = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idCity = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.idDistrict);
                    table.ForeignKey(
                        name: "FK_District_City_idCity",
                        column: x => x.idCity,
                        principalTable: "City",
                        principalColumn: "idCity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    idDiscount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    discountPercent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.idDiscount);
                    table.ForeignKey(
                        name: "FK_Discount_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageProduct",
                columns: table => new
                {
                    idImageProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageProduct", x => x.idImageProduct);
                    table.ForeignKey(
                        name: "FK_ImageProduct_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductColor",
                columns: table => new
                {
                    idColor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColor", x => new { x.idColor, x.idProduct });
                    table.ForeignKey(
                        name: "FK_ProductColor_Color_idColor",
                        column: x => x.idColor,
                        principalTable: "Color",
                        principalColumn: "idColor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductColor_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSIze",
                columns: table => new
                {
                    idSize = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSIze", x => new { x.idSize, x.idProduct });
                    table.ForeignKey(
                        name: "FK_ProductSIze_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSIze_Size_idSize",
                        column: x => x.idSize,
                        principalTable: "Size",
                        principalColumn: "idSize",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailVoteWarehouse",
                columns: table => new
                {
                    idVotewarehouse = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    purchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailVoteWarehouse", x => new { x.idVotewarehouse, x.idProduct });
                    table.ForeignKey(
                        name: "FK_DetailVoteWarehouse_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailVoteWarehouse_VoteWarehouse_idVotewarehouse",
                        column: x => x.idVotewarehouse,
                        principalTable: "VoteWarehouse",
                        principalColumn: "idVotewarehouse",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ward",
                columns: table => new
                {
                    idWard = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameWard = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idDistrict = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ward", x => x.idWard);
                    table.ForeignKey(
                        name: "FK_Ward_District_idDistrict",
                        column: x => x.idDistrict,
                        principalTable: "District",
                        principalColumn: "idDistrict",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    idCustomer = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameCustomer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    userCustomer = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    passwordCustomer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dateBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idCountry = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idCity = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idDistrict = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idWard = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    OtpCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    OtpExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.idCustomer);
                    table.ForeignKey(
                        name: "FK_Customer_City_idCity",
                        column: x => x.idCity,
                        principalTable: "City",
                        principalColumn: "idCity");

                    table.ForeignKey(
                        name: "FK_Customer_Country_idCountry",
                        column: x => x.idCountry,
                        principalTable: "Country",
                        principalColumn: "idCountry");

                    table.ForeignKey(
                        name: "FK_Customer_District_idDistrict",
                        column: x => x.idDistrict,
                        principalTable: "District",
                        principalColumn: "idDistrict");

                    table.ForeignKey(
                        name: "FK_Customer_Ward_idWard",
                        column: x => x.idWard,
                        principalTable: "Ward",
                        principalColumn: "idWard");
                   
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    idOrder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idCustomer = table.Column<int>(type: "int", nullable: false),
                    dateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    statusOrder = table.Column<int>(type: "int", nullable: true),
                    statusPay = table.Column<int>(type: "int", nullable: true),
                    totalOrder = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.idOrder);
                    table.ForeignKey(
                        name: "FK_Order_Customer_idCustomer",
                        column: x => x.idCustomer,
                        principalTable: "Customer",
                        principalColumn: "idCustomer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailOrder",
                columns: table => new
                {
                    idOrder = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    nameSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nameColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailOrder", x => new { x.idOrder, x.idProduct });
                    table.ForeignKey(
                        name: "FK_DetailOrder_Order_idOrder",
                        column: x => x.idOrder,
                        principalTable: "Order",
                        principalColumn: "idOrder",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailOrder_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_idCountry",
                table: "City",
                column: "idCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_idCity",
                table: "Customer",
                column: "idCity");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_idCountry",
                table: "Customer",
                column: "idCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_idDistrict",
                table: "Customer",
                column: "idDistrict");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_idWard",
                table: "Customer",
                column: "idWard");

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrder_idProduct",
                table: "DetailOrder",
                column: "idProduct");

            migrationBuilder.CreateIndex(
                name: "IX_DetailVoteWarehouse_idProduct",
                table: "DetailVoteWarehouse",
                column: "idProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_idProduct",
                table: "Discount",
                column: "idProduct");

            migrationBuilder.CreateIndex(
                name: "IX_District_idCity",
                table: "District",
                column: "idCity");

            migrationBuilder.CreateIndex(
                name: "IX_ImageProduct_idProduct",
                table: "ImageProduct",
                column: "idProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Order_idCustomer",
                table: "Order",
                column: "idCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_Product_idBranch",
                table: "Product",
                column: "idBranch");

            migrationBuilder.CreateIndex(
                name: "IX_Product_idCategory",
                table: "Product",
                column: "idCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Product_idPet",
                table: "Product",
                column: "idPet");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_idProduct",
                table: "ProductColor",
                column: "idProduct");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSIze_idProduct",
                table: "ProductSIze",
                column: "idProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Role_AdminidAdmin",
                table: "Role",
                column: "AdminidAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Role_idMission",
                table: "Role",
                column: "idMission");

            migrationBuilder.CreateIndex(
                name: "IX_VoteWarehouse_idSupplier",
                table: "VoteWarehouse",
                column: "idSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_Ward_idDistrict",
                table: "Ward",
                column: "idDistrict");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailOrder");

            migrationBuilder.DropTable(
                name: "DetailVoteWarehouse");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "ImageProduct");

            migrationBuilder.DropTable(
                name: "ProductColor");

            migrationBuilder.DropTable(
                name: "ProductSIze");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "VoteWarehouse");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Mission");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "Ward");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
