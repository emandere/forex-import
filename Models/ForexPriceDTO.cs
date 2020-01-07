using System.Text.Json.Serialization;
namespace forex_import.Models
{
    public  class ForexPriceDTO
    {
        [JsonPropertyName("Instrument")]
        public string Instrument { get; set; }

        [JsonPropertyName("Time")]

        public string Time { get; set; }  

        [JsonPropertyName("Bid")]
 
        public double Bid { get; set; }

        [JsonPropertyName("Ask")]
        public double Ask { get; set; }
    }   
}