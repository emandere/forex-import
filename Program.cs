using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AutoMapper;
using forex_import.Models;
using forex_import.Config;

namespace forex_import
{
    
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static List<string> pairs = new List<string>()
        {
            "AUDUSD",
            "EURUSD",
            "GBPUSD",
            "NZDUSD",
            "USDCAD",
            "USDCHF",
            "USDJPY"
        };

        static async Task Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ForexPriceProfile());
                cfg.AddProfile(new ForexSessionProfile());
            });
            var mapper = new Mapper(config);
            client.Timeout = TimeSpan.FromMinutes(10);    

            string serverLocal = configuration.GetSection("Servers:Local").Value;
            string server = configuration.GetSection("Servers:Remote").Value;
            
            Console.WriteLine($"Environment: {env} and API's: {serverLocal} and {server}");

            while(true)
            {
                try
                {
                    await UpdateLocal(server,serverLocal);
                    Console.WriteLine("Updated...");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Graceful exception:{e.Message}");
                }
                await Task.Delay(1000*60*1);
                
            }
            
        }

        static async Task UpdateLocal(string server,string serverLocal)
        {
            
           
            var pricesLocal = await GetDailyPricesFromLocal(serverLocal);
            var shouldUpdate = false;

            if(pricesLocal.priceDTOs.Count()==0)
            {
                shouldUpdate = true;
            }
            
            foreach(var price in pricesLocal.priceDTOs)
            {
                var serverPrice = await GetLatestPricesDTO(server,price.Instrument);
                if(serverPrice.Item1.UTCTime.CompareTo(price.UTCTimeAddZ)>0)
                {
                    shouldUpdate = true;
                }
                else
                {
                    Console.WriteLine($"{price.Instrument} Not updated");
                }
            }

            if(shouldUpdate)
            {
                foreach(var pair in pairs)
                {
                    var serverPrice = await GetLatestPricesDTO(server,pair);
                    await SaveRealTimePrices(serverLocal,pair,serverPrice.Item2);
                    Console.WriteLine($"{pair} Updated");
                }
                var sessionsLocal = await GetSessions(server);
                await SaveSessions(serverLocal,sessionsLocal);
                await SaveAllDailyRealTimePrices(server,serverLocal);
            }

        }

       

        static async Task<string> GetDailyPrices(string startDate, string endDate,string server,string pair)
        {
            string url = $"http://{server}/api/forexclasses/v1/dailypricesrange/{pair}/{startDate}/{endDate}";
            string responseBody = await client.GetStringAsync(url);
            //Console.WriteLine(responseBody);
            return responseBody;
        }

        static async Task<string> GetDailyRealTimePrices(string startDate,string server,string pair)
        {
            string url = $"http://{server}/api/forexclasses/v1/dailyrealtimeprices/{pair}/{startDate}";
            string responseBody = await client.GetStringAsync(url);
            //Console.WriteLine(responseBody);
            return responseBody;
        }

        static async Task<string> GetSessions(string server)
        {
            string url = $"http://{server}/api/forexclasses/v1/sessions";
            string responseBody = await client.GetStringAsync(url);
            //Console.WriteLine(responseBody);
            return responseBody; 
        }

        static async Task<(ForexPriceDTO,string)> GetLatestPricesDTO(string server, string pair)
        {
            string url = $"http://{server}/api/forexclasses/v1/latestprices/{pair}";
            string responseBody = await client.GetStringAsync(url);

            var priceLocal = JsonSerializer.Deserialize<ForexPriceDTO>(responseBody);

            return (priceLocal,responseBody);
        }

        static async Task<ForexDailyPriceDTO> GetLatestDailyPriceDTO(string server, string pair)
        {
            string url = $"http://{server}/api/forexdailyprices/{pair}";
            string responseBody = await client.GetStringAsync(url);

            var priceLocal = JsonSerializer.Deserialize<ForexDailyPriceDTO>(responseBody);

            return priceLocal;
        }

        static async Task<PricesDTO> GetDailyPricesFromLocal(string server)
        {
            string url = $"http://{server}/api/forexprices";
            string responseBody = await client.GetStringAsync(url);
            var pricesLocal = JsonSerializer.Deserialize<PricesDTO>(responseBody);
            //var compare = pricesLocal.priceDTOs[0].UTCTime.CompareTo(DateTime.Now);
            //Console.WriteLine(pricesLocal.priceDTOs[0].Instrument);
            return pricesLocal;
        }
        static async Task SaveDailyPrices(string server,string prices)
        {
            string urlPost = $"http://{server}/api/forexdailyprices/";
            var stringContent = new StringContent(prices,UnicodeEncoding.UTF8, "application/json");
            var responseBodyPost = await client.PostAsync(urlPost,stringContent);
        }

        static async Task SaveDailyRealPrices(string server,string prices)
        {
            string urlPost = $"http://{server}/api/forexdailyrealprices";
            var stringContent = new StringContent(prices,UnicodeEncoding.UTF8, "application/json");
            var responseBodyPost = await client.PostAsync(urlPost,stringContent);
        }

        static async Task SaveRealTimePrices(string server,string pair,string responseBody)
        {
            string urlPost = $"http://{server}/api/forexprices/{pair}";
            var stringContent = new StringContent(responseBody,UnicodeEncoding.UTF8, "application/json");
            var responseBodyPost = await client.PutAsync(urlPost,stringContent);
        }

         static async Task SaveSessions(string server,string sessions)
        {
            string urlPost = $"http://{server}/api/forexsession/";
            var stringContent = new StringContent(sessions,UnicodeEncoding.UTF8, "application/json");
            var responseBodyPost = await client.PostAsync(urlPost,stringContent);
        }


        static async Task SavePairDailyRealTimePrice(string pair,string server,string serverLocal)
        {
            var startDate = "20160101";
            string endDate = "20300101";
            Console.WriteLine($"Adding Real Prices for {pair}");

            try
            {
                var latestDailyPrice = await GetLatestDailyPriceDTO(serverLocal,pair);
                if(latestDailyPrice !=null)
                    startDate = DateTime.Parse(latestDailyPrice.Datetime).AddDays(-1).ToString("yyyyMMdd");
            }
            catch(Exception exp)
            {
                startDate = "20160101";
                Console.WriteLine(exp.Message);
            }

            
            var dailyPrices = await GetDailyPrices(startDate,endDate,server,pair);
            await SaveDailyPrices(serverLocal,dailyPrices);
            /*var dailyPricesDTO = JsonSerializer.Deserialize<List<ForexDailyPriceDTO>>(dailyPrices);
            foreach(var price in dailyPricesDTO)
            {
                Console.WriteLine($"{pair} {price.DateTimeDayOnly}");
                var dailyRealPrices = await GetDailyRealTimePrices(price.DateTimeDayOnly,server,price.Pair);
                await SaveDailyRealPrices(serverLocal,dailyRealPrices);
            }*/
        }

         static async Task SaveAllDailyRealTimePrices(string server,string serverLocal)
         {
            var saveTasks = pairs.Select(x => SavePairDailyRealTimePrice(x,server,serverLocal));
            await Task.WhenAll(saveTasks);
         }
    }
}
