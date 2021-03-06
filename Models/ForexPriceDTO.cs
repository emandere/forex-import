using System;
using System.Text.Json.Serialization;
namespace forex_import.Models
{
    public class ForexPricesDTO
    {
        [JsonPropertyName("prices")]
        public ForexPriceDTO [] priceDTOs{get;set;}
    }
    public  class ForexPriceDTO
    {
        [JsonPropertyName("instrument")]
        public string Instrument { get; set; }

        [JsonPropertyName("time")]

        public string Time { get; set; }  

        [JsonPropertyName("bid")]
 
        public double Bid { get; set; }

        [JsonPropertyName("ask")]
        public double Ask { get; set; }

        public DateTime UTCTime{get => DateTime.Parse(Time);}

        //public DateTime UTCTimeAddZ{get => DateTime.Parse(Time+"Z");}
    }   
}