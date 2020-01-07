using System;
namespace forex_import.Domain
{
    public  class ForexPrice
    {
        public string Instrument { get; set; }
        public string Time { get; set; }   
        public double Bid { get; set; }
        public double Ask { get; set; }
    }   
}