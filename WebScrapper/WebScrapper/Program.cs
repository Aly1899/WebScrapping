using System;
using System.Threading.Tasks;

namespace WebScrapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Scrapper.GetAdsData();
            Console.WriteLine();
        }
    }
}
