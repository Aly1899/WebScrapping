using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Context;
using WebScrapper.Models;

namespace WebScrapper
{
    public class Scrapper
    {
        public static async Task GetAdsData()
        {

            using (var client = new HttpClient())
            {
                using (var context = new RealEstateContext())
                {
                    using var transaction = context.Database.BeginTransaction();

                    //string url = "https://ingatlan.com/szukites/elado+lakas+budapest+30-50-mFt+4-szoba-felett";
                    string url = "https://ingatlan.com/lista/elado+telek+50-mFt-ig";
                    var pages = await GetMaxPage(client, url);
                
                    var ads = new List<RealEstate>();

                    //for (var p = 1; p <= pages; p++)
                    var lastFetchDate = await context.FetchDates.OrderByDescending(f => f.EntryDate).FirstOrDefaultAsync();
                    var newFetchId = lastFetchDate==null?1:lastFetchDate.Id+1;
                    for (var p = 1; p <= 1; p++)
                        {
                        Console.WriteLine($"Page: {p}");
                        var htmlDoc = await GetAdHtmlDocument(client, url, p);
                        var nodes = htmlDoc.DocumentNode.SelectNodes(".//div[contains(@class,'listing ')]");
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            FillAd(transaction, context, nodes[i],newFetchId);
                            
                        }
                    }
                    var t = context.ChangeTracker.HasChanges();
                    context.ChangeTracker.DetectChanges();
                    
                    if (context.ChangeTracker.HasChanges())
                    {
                        var fetchDate = new FetchDate()
                        {
                            Id = newFetchId,
                            EntryDate = DateTime.UtcNow.Date
                        };
                        context.Add(fetchDate);
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }

            }
        }
        private static void FillAd(IDbContextTransaction transaction, RealEstateContext reContext, HtmlNode node, int newFetchId)
        {
            //using var priceContext = new AdPriceContext();
            var ad = new RealEstate();
            var adId = node.GetAttributeValue("data-id", "").ToString();
            var address = node.SelectSingleNode(".//div[@class = 'listing__address']");
            var findAd = reContext.RealEstates.FirstOrDefault(r => r.AdId == adId && r.Address == address.InnerText.Trim());
            if (findAd == null)
            {
                ad.RealEstateId = Guid.NewGuid();
                ad.AdId = adId;

                ad.AdType = "lakás";
                ad.AdType = "telek";
                if (address != null)
                {
                    ad.Address = address.InnerText.Trim();
                    var commaIndex = address.InnerText.LastIndexOf(",");
                    if (commaIndex == -1)
                    {
                        ad.City = address.InnerText.Trim();

                    } else
                    {
                        ad.City = address.InnerText.Substring(commaIndex+1).Trim();
                    }
                }

                var price = node.SelectSingleNode(".//div[@class = 'price']");
                if (price != null)
                {
                    var newPrince = new AdPrice()
                    {
                        AdPriceId = new Guid(),
                        RealEstateId = ad.RealEstateId,
                        AdId= ad.AdId,
                        OldPrice = Convert.ToDecimal(price.InnerText.Split(" ")[1]),
                        NewPrice = Convert.ToDecimal(price.InnerText.Split(" ")[1]),
                        EntryDate = DateTime.UtcNow,
                        FetchId = newFetchId
                    };
                    reContext.Add(newPrince);
                    reContext.SaveChanges();
                }

                var area = node.SelectSingleNode(".//div[contains(@class , 'listing__data--area-size')]");
                if (area != null)
                {
                    ad.Area = Convert.ToDecimal(area.InnerText.Split(" ")[1]);
                }

                var plotSize = node.SelectSingleNode(".//div[contains(@class , 'listing__data--plot-size')]");
                if (plotSize != null)
                {
                    var plotSizeTxt = plotSize.InnerText.ToString();
                    ad.PlotSize = Convert.ToInt32(plotSizeTxt.Substring(0, plotSizeTxt.Count() - 9).Replace(" ", ""));
                }

                //ad.PricePerSqm = Math.Round((ad.Price.Value / ad.Area.Value) * 1000000, 2);

                var balcony = node.SelectSingleNode(".//div[contains(@class,'listing__data--balcony-size')]");
                if (balcony != null)
                {
                    ad.Balcony = balcony.InnerText;
                }
                var leasing = node.SelectSingleNode(".//div[contains(@class , 'label--alert')]");
                ad.LeaseRights = leasing == null ? false : true;
                ad.Date = DateTime.UtcNow;
                Console.WriteLine($"{ad.AdId} added");
                reContext.RealEstates.Add(ad);
                reContext.Entry(ad).State = EntityState.Added;
                reContext.SaveChanges();
                var e = reContext.ChangeTracker.Entries();
                DisplayStates(e);
            } else
            {
                var adPrice = reContext.AdPrices
                    .Where(a => a.AdId == findAd.AdId)
                    .OrderByDescending(a => a.EntryDate)
                    .Take(1)
                    .ToList();

                var price = node.SelectSingleNode(".//div[@class = 'price']");
                if (price != null)
                {
                    var actualPrice = Convert.ToDecimal(price.InnerText.Split(" ")[1]);
                    if (adPrice[0].NewPrice != actualPrice)
                    {
                        var newPrice = new AdPrice()
                        {
                            AdPriceId = new Guid(),
                            RealEstateId = findAd.RealEstateId,
                            AdId = findAd.AdId,
                            OldPrice = adPrice[0].NewPrice,
                            NewPrice = actualPrice,
                            EntryDate = DateTime.UtcNow,
                            FetchId = newFetchId
                        };
                        Console.WriteLine($"Price update of : {findAd.RealEstateId}");
                        reContext.Add(newPrice);
                        reContext.SaveChanges();
                    }
                }
                
            }
            
        }
        public static async Task<HtmlDocument> GetAdHtmlDocument(HttpClient client, string url, int page)
        {
            var fullUrl = $"{url}?page={page}";
            var response = await client.GetAsync(fullUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new HtmlDocument();
            }
            var htmlBody = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlBody);
            return htmlDoc;
        }
        public static async Task<int> GetMaxPage(HttpClient client, string url)
        {
            var htmlDocForPages = await GetAdHtmlDocument(client, url, 1);
            var pagesTxt = htmlDocForPages.DocumentNode.SelectSingleNode(".//*[contains(@class,'pagination__page-number')]").InnerText;
            int maxPage = Convert.ToInt32(pagesTxt.Split(" ")[3]);
            return maxPage;
        }

        public static void DisplayStates(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name},State: { entry.State.ToString()}");
            }
        }
    }
}
