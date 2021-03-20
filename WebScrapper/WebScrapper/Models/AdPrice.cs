using System;
using System.Collections.Generic;
using System.Text;

namespace WebScrapper.Models
{
    public class AdPrice
    {
        public Guid AdPriceId { get; set; }
        public Guid RealEstateId { get; set; }
        public string? AdId { get; set; }
        public Decimal? OldPrice { get; set; }
        public Decimal? NewPrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public int FetchId { get; set; }
    }
}

