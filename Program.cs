﻿using System;
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
using forex_import.Models.Input;
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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            string server = "23.22.66.239";
            string url = $"http://{server}/api/forexclasses/v1/latestprices/AUDUSD";
            string responseBody = await client.GetStringAsync(url);
            Console.WriteLine(responseBody);

            string serverLocal = "localhost:5002";
            string urlPost = $"http://{serverLocal}/api/forexprices/AUDUSD";
            var stringContent = new StringContent(responseBody,UnicodeEncoding.UTF8, "application/json");
            var responseBodyPost = await client.PutAsync(urlPost,stringContent);

            string startDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string endDate = "20300101";

            var dailyPrices = await GetDailyPrices(startDate,endDate,server,"AUDUSD");
            await SaveDailyPrices(serverLocal,dailyPrices);

            var pricesLocal = await GetDailyPricesFromLocal(serverLocal);
            var sessionsLocal = await GetSessions(server);
            var sessions = JsonSerializer.Deserialize<List<ForexSessionInDTO>>(sessionsLocal);
            await SaveSessions(serverLocal,sessionsLocal);
            foreach(var price in pricesLocal.priceDTOs)
            {
                var serverPrice = await GetLatestPricesDTO(server,price.Instrument);
                if(serverPrice.Item1.UTCTime.CompareTo(price.UTCTimeAddZ)>0)
                {
                    await SaveRealTimePrices(serverLocal,price.Instrument,serverPrice.Item2);
                    Console.WriteLine($"{price.Instrument} Updated");
                }
                else
                {
                    Console.WriteLine($"{price.Instrument} Not updated");
                }
            }

        }

        static async Task<string> GetDailyPrices(string startDate, string endDate,string server,string pair)
        {
            string url = $"http://{server}/api/forexclasses/v1/dailypricesrange/{pair}/{startDate}/{endDate}";
            string responseBody = await client.GetStringAsync(url);
            Console.WriteLine(responseBody);
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
    }
}
