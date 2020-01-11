using System;
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
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            string server = "23.22.66.239";
            string url = $"http://{server}/api/forexclasses/v1/latestprices/AUDUSD";
            string responseBody = await client.GetStringAsync(url);
            Console.WriteLine(responseBody);

            string serverPost = "localhost:5002";
            string urlPost = $"http://{serverPost}/api/forexprices/AUDUSD";
            var stringContent = new StringContent(responseBody,UnicodeEncoding.UTF8, "application/json");
            var responseBodyPost = await client.PutAsync(urlPost,stringContent);

            string startDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string endDate = "20300101";

            var dailyPrices = await GetDailyPrices(startDate,endDate,server,"AUDUSD");
            await SaveDailyPrices(serverPost,dailyPrices);

        }

        static async Task<string> GetDailyPrices(string startDate, string endDate,string server,string pair)
        {
            string url = $"http://{server}/api/forexclasses/v1/dailypricesrange/{pair}/{startDate}/{endDate}";
            string responseBody = await client.GetStringAsync(url);
            Console.WriteLine(responseBody);
            return responseBody;
        }

        static async Task SaveDailyPrices(string server,string prices)
        {
            string urlPost = $"http://{server}/api/forexdailyprices/";
            var stringContent = new StringContent(prices,UnicodeEncoding.UTF8, "application/json");
            var responseBodyPost = await client.PostAsync(urlPost,stringContent);
        }
    }
}
