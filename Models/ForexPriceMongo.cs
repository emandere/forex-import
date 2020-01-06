using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace forex_import.Models
{
    public  class ForexPriceMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("instrument")]
        public string Instrument { get; set; }

        [BsonElement("time")]
        public DateTime Time { get; set; }

        [BsonElement("bid")]
        public double Bid { get; set; }

        [BsonElement("ask")]
        public double Ask { get; set; }
    }   
}
