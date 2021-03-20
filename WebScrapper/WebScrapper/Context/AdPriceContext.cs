//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using WebScrapper.Models;

//namespace WebScrapper.Context
//{
//    class AdPriceContext: DbContext
//    {
//        public DbSet<AdPrice> AdPrices { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<AdPrice>().HasKey(a=>a.AdPriceId);
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlServer("Data Source=DESKTOP-V9BTOJH\\SQLEXPRESS;Initial Catalog=realestate;Integrated Security=True");
//        }
//    }
//}
