﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebThuCung.Data;

#nullable disable

namespace WebThuCung.Migrations
{
    [DbContext(typeof(PetContext))]
    [Migration("20241021105359_fiximageproduct")]
    partial class fiximageproduct
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebThuCung.Models.Admin", b =>
                {
                    b.Property<string>("idAdmin")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("passwordAdmin")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("userAdmin")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("idAdmin");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("WebThuCung.Models.Branch", b =>
                {
                    b.Property<string>("idBranch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("nameBranch")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("idBranch");

                    b.ToTable("Branch");
                });

            modelBuilder.Entity("WebThuCung.Models.Category", b =>
                {
                    b.Property<string>("idCategory")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("nameCategory")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("idCategory");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("WebThuCung.Models.Color", b =>
                {
                    b.Property<string>("idColor")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("nameColor")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("idColor");

                    b.ToTable("Color");
                });

            modelBuilder.Entity("WebThuCung.Models.Customer", b =>
                {
                    b.Property<int>("idCustomer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idCustomer"));

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("OtpCode")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime?>("OtpExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime?>("dateBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("nameCustomer")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("passwordCustomer")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("userCustomer")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("idCustomer");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("WebThuCung.Models.DetailOrder", b =>
                {
                    b.Property<string>("idOrder")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<string>("idProduct")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1);

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal?>("totalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("idOrder", "idProduct");

                    b.HasIndex("idProduct");

                    b.ToTable("DetailOrder");
                });

            modelBuilder.Entity("WebThuCung.Models.DetailVoteWarehouse", b =>
                {
                    b.Property<string>("idVotewarehouse")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<string>("idProduct")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1);

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("purchasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("totalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("idVotewarehouse", "idProduct");

                    b.HasIndex("idProduct");

                    b.ToTable("DetailVoteWarehouse");
                });

            modelBuilder.Entity("WebThuCung.Models.Discount", b =>
                {
                    b.Property<string>("idDiscount")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("discountPercent")
                        .HasColumnType("int");

                    b.Property<string>("idProduct")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("idDiscount");

                    b.HasIndex("idProduct");

                    b.ToTable("Discount");
                });

            modelBuilder.Entity("WebThuCung.Models.ImageProduct", b =>
                {
                    b.Property<string>("idImageProduct")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("idProduct")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("idImageProduct");

                    b.HasIndex("idProduct");

                    b.ToTable("ImageProduct");
                });

            modelBuilder.Entity("WebThuCung.Models.Mission", b =>
                {
                    b.Property<string>("idMission")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("nameMission")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("idMission");

                    b.ToTable("Mission");
                });

            modelBuilder.Entity("WebThuCung.Models.Order", b =>
                {
                    b.Property<string>("idOrder")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("dateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("idCustomer")
                        .HasColumnType("int");

                    b.Property<bool?>("statusOrder")
                        .HasColumnType("bit");

                    b.Property<bool?>("statusPay")
                        .HasColumnType("bit");

                    b.Property<decimal?>("totalOrder")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("idOrder");

                    b.HasIndex("idCustomer");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("WebThuCung.Models.Pet", b =>
                {
                    b.Property<string>("idPet")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("namePet")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("idPet");

                    b.ToTable("Pet");
                });

            modelBuilder.Entity("WebThuCung.Models.Product", b =>
                {
                    b.Property<string>("idProduct")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PetidPet")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("idBranch")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("idCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("idColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("idPet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nameProduct")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal?>("sellPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("idProduct");

                    b.HasIndex("PetidPet");

                    b.HasIndex("idBranch");

                    b.HasIndex("idCategory");

                    b.HasIndex("idColor");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("WebThuCung.Models.Role", b =>
                {
                    b.Property<int>("idAdmin")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("idRole")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<string>("AdminidAdmin")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("idMission")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("idAdmin", "idRole");

                    b.HasIndex("AdminidAdmin");

                    b.HasIndex("idMission");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("WebThuCung.Models.Size", b =>
                {
                    b.Property<string>("idSize")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("idProduct")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("nameSize")
                        .HasColumnType("int");

                    b.HasKey("idSize");

                    b.HasIndex("idProduct");

                    b.ToTable("Size");
                });

            modelBuilder.Entity("WebThuCung.Models.Supplier", b =>
                {
                    b.Property<string>("idSupplier")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("nameSupplier")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("idSupplier");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("WebThuCung.Models.VoteWarehouse", b =>
                {
                    b.Property<string>("idVotewarehouse")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("dateEntry")
                        .HasColumnType("datetime2");

                    b.Property<string>("idSupplier")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("totalVoteWarehouse")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("idVotewarehouse");

                    b.HasIndex("idSupplier");

                    b.ToTable("VoteWarehouse");
                });

            modelBuilder.Entity("WebThuCung.Models.DetailOrder", b =>
                {
                    b.HasOne("WebThuCung.Models.Order", "Order")
                        .WithMany("DetailOrders")
                        .HasForeignKey("idOrder")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebThuCung.Models.Product", "Product")
                        .WithMany("DetailOrders")
                        .HasForeignKey("idProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebThuCung.Models.DetailVoteWarehouse", b =>
                {
                    b.HasOne("WebThuCung.Models.Product", "Product")
                        .WithMany("DetailVoteWarehouses")
                        .HasForeignKey("idProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebThuCung.Models.VoteWarehouse", "VoteWarehouse")
                        .WithMany("DetailVoteWarehouses")
                        .HasForeignKey("idVotewarehouse")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("VoteWarehouse");
                });

            modelBuilder.Entity("WebThuCung.Models.Discount", b =>
                {
                    b.HasOne("WebThuCung.Models.Product", "Product")
                        .WithMany("Discounts")
                        .HasForeignKey("idProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebThuCung.Models.ImageProduct", b =>
                {
                    b.HasOne("WebThuCung.Models.Product", "Product")
                        .WithMany("ImageProducts")
                        .HasForeignKey("idProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebThuCung.Models.Order", b =>
                {
                    b.HasOne("WebThuCung.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("idCustomer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("WebThuCung.Models.Product", b =>
                {
                    b.HasOne("WebThuCung.Models.Pet", "Pet")
                        .WithMany("Products")
                        .HasForeignKey("PetidPet");

                    b.HasOne("WebThuCung.Models.Branch", "Branch")
                        .WithMany("Products")
                        .HasForeignKey("idBranch")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebThuCung.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("idCategory")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebThuCung.Models.Color", "Color")
                        .WithMany("Products")
                        .HasForeignKey("idColor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Category");

                    b.Navigation("Color");

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("WebThuCung.Models.Role", b =>
                {
                    b.HasOne("WebThuCung.Models.Admin", "Admin")
                        .WithMany("Roles")
                        .HasForeignKey("AdminidAdmin");

                    b.HasOne("WebThuCung.Models.Mission", "Mission")
                        .WithMany("Roles")
                        .HasForeignKey("idMission")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Mission");
                });

            modelBuilder.Entity("WebThuCung.Models.Size", b =>
                {
                    b.HasOne("WebThuCung.Models.Product", "Product")
                        .WithMany("Sizes")
                        .HasForeignKey("idProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebThuCung.Models.VoteWarehouse", b =>
                {
                    b.HasOne("WebThuCung.Models.Supplier", "Supplier")
                        .WithMany("VoteWarehouses")
                        .HasForeignKey("idSupplier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("WebThuCung.Models.Admin", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("WebThuCung.Models.Branch", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebThuCung.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebThuCung.Models.Color", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebThuCung.Models.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("WebThuCung.Models.Mission", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("WebThuCung.Models.Order", b =>
                {
                    b.Navigation("DetailOrders");
                });

            modelBuilder.Entity("WebThuCung.Models.Pet", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebThuCung.Models.Product", b =>
                {
                    b.Navigation("DetailOrders");

                    b.Navigation("DetailVoteWarehouses");

                    b.Navigation("Discounts");

                    b.Navigation("ImageProducts");

                    b.Navigation("Sizes");
                });

            modelBuilder.Entity("WebThuCung.Models.Supplier", b =>
                {
                    b.Navigation("VoteWarehouses");
                });

            modelBuilder.Entity("WebThuCung.Models.VoteWarehouse", b =>
                {
                    b.Navigation("DetailVoteWarehouses");
                });
#pragma warning restore 612, 618
        }
    }
}
