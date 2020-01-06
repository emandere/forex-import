using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using forex_import.Models;

namespace forex_import
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var serviceProvider = new ServiceCollection()
                .AddAutoMapper()
                .Configure<Settings>(options =>
                {
                    options.ConnectionString 
                        = configuration.GetSection("MongoConnection:ConnectionString").Value;
                    options.Database 
                        = configuration.GetSection("MongoConnection:Database").Value;
                })
                .BuildServiceProvider(); 
            Console.WriteLine("Hello World!");
        }
    }
}
