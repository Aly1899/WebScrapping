using System;
using System.Collections.Generic;
using System.Text;

namespace WebScrapper.Models
{
    public class RealEstate
    {
        public Guid RealEstateId { get; set; }
        public string? AdId { get; set; }
        public string? AdType { get; set; }
        public Decimal? Price { get; set; }
        public Decimal? PricePerSqm { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public Decimal? Area { get; set; }
        public int? PlotSize { get; set; }
        public bool LeaseRights { get; set; }
        public string? Balcony { get; set; }
        public DateTime? Date { get; set; }
    }
}

