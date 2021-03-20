using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebScrapper.Models;

namespace WebScrapper.Context
{
    class RealEstateContext: DbContext
    {
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<FetchDate> FetchDates { get; set; }
        public DbSet<AdPrice> AdPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstate>().HasKey(r=>r.RealEstateId);
            modelBuilder.Entity<FetchDate>().HasKey(f=>f.Id);
            modelBuilder.Entity<AdPrice>().HasKey(a=>a.AdPriceId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-V9BTOJH\\SQLEXPRESS;Initial Catalog=realestate;Integrated Security=True");
        }
    }
}
