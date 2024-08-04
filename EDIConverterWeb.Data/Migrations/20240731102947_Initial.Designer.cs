﻿// <auto-generated />
using System;
using EDIConverterWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EDIConverterWeb.Data.Migrations
{
    [DbContext(typeof(EDIDbContext))]
    [Migration("20240731102947_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EDIConverterWeb.Data.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("ItemNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PurchaseOrderAcknowledgementId")
                        .HasColumnType("int");

                    b.Property<int>("QuantityOrdered")
                        .HasColumnType("int");

                    b.Property<string>("UnitOfMeasure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnitPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseOrderAcknowledgementId");

                    b.ToTable("ItemsOrdered");
                });

            modelBuilder.Entity("EDIConverterWeb.Data.PurchaseOrderAcknowledgement", b =>
                {
                    b.Property<int>("InterchangeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InterchangeId"), 100010001L, 10001);

                    b.Property<DateTime>("AcknowledgementDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PurchaseOrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PurchaseOrderNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ScheduledShipDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TestIndicator")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("TransactionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InterchangeId");

                    b.ToTable("PurchaseOrderAcknowledgements", (string)null);
                });

            modelBuilder.Entity("EDIConverterWeb.Data.Item", b =>
                {
                    b.HasOne("EDIConverterWeb.Data.PurchaseOrderAcknowledgement", "PurchaseOrderAcknowledgement")
                        .WithMany("ItemsOrdered")
                        .HasForeignKey("PurchaseOrderAcknowledgementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseOrderAcknowledgement");
                });

            modelBuilder.Entity("EDIConverterWeb.Data.PurchaseOrderAcknowledgement", b =>
                {
                    b.Navigation("ItemsOrdered");
                });
#pragma warning restore 612, 618
        }
    }
}