using MongoDB.Bson.Serialization.Attributes;
namespace forex_import.Models
{
   public class StrategyMongo
   {
       [BsonElement("window")]
        public int window{get;set;}
        [BsonElement("ruleName")]
        public string ruleName{get;set;}
        [BsonElement("position")]
        public string position{get;set;}
        [BsonElement("units")]
        public int units{get;set;}
        [BsonElement("stopLoss")]
        public double stopLoss{get;set;}
        [BsonElement("takeProfit")]
        public double takeProfit{get;set;}

   } 
}