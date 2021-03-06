// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebScrapper.Context;

namespace WebScrapper.Migrations
{
    [DbContext(typeof(RealEstateContext))]
    partial class RealEstateContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebScrapper.Models.AdPrice", b =>
                {
                    b.Property<Guid>("AdPriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FetchId")
                        .HasColumnType("int");

                    b.Property<decimal?>("NewPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("OldPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("RealEstateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdPriceId");

                    b.ToTable("AdPrices");
                });

            modelBuilder.Entity("WebScrapper.Models.FetchDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("Date");

                    b.HasKey("Id");

                    b.ToTable("FetchDates");
                });

            modelBuilder.Entity("WebScrapper.Models.RealEstate", b =>
                {
                    b.Property<Guid>("RealEstateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Area")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Balcony")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LeaseRights")
                        .HasColumnType("bit");

                    b.Property<int?>("PlotSize")
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PricePerSqm")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("RealEstateId");

                    b.ToTable("RealEstates");
                });
#pragma warning restore 612, 618
        }
    }
}
