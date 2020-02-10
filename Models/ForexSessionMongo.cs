using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace forex_import.Models
{
    public  class ForexSessionMongo
    {
        //[BsonElement("_id")]
        //public string _Id { get; set; }

        //[BsonElement("id")]
        //public string id { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("id")]
        public string idinfo { get; set; }

        [BsonElement("sessionType")]
        public string SessionType { get; set; }

        [BsonElement("experimentId")]
        public string ExperimentId { get; set; }

        [BsonElement("startDate")]
        public string StartDate { get; set; }

        [BsonElement("endDate")]
        public string EndDate { get; set; }

        [BsonElement("lastUpdatedTime")]
        public string LastUpdatedTime { get; set; }

        [BsonElement("currentTime")]
        public string CurrentTime { get; set; }

        [BsonElement("strategy")]
        public StrategyMongo Strategy { get; set; }

        [BsonElement("sessionUser")]
        public SessionUserMongo SessionUser { get; set; }

        [BsonElement("percentComplete")]
        public string PercentComplete { get; set; }

        [BsonElement("elapsedTime")]
        public string elapsedTime { get; set; }
        [BsonElement("beginSessionTime")]
        public string beginSessionTime { get; set; }
        [BsonElement("endSessionTime")]
        public string endSessionTime { get; set; }
    }

    public  class SessionUserMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("id")]
        public string idinfo { get; set; }

        [BsonElement("status")]
        public object Status { get; set; }

        [BsonElement("Accounts")]
        public AccountsMongo Accounts { get; set; }
    }

    public  class AccountsMongo
    {
        
        [BsonElement("primary")]
        public AccountMongo Primary { get; set; }

        [BsonElement("secondary")]
        public AccountMongo Secondary { get; set; }
    }

    public  class AccountMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("id")]
        public string idinfo { get; set; }

        [BsonElement("cash")]
        public double Cash { get; set; }

        [BsonElement("Margin")]
        public long Margin { get; set; }

        [BsonElement("MarginRatio")]
        public long MarginRatio { get; set; }

        [BsonElement("realizedPL")]
        public double RealizedPl { get; set; }

        [BsonElement("Trades")]
        public TradeMongo[] Trades { get; set; }

        [BsonElement("orders")]
        public OrderMongo[] Orders { get; set; }

        [BsonElement("closedTrades")]
        public TradeMongo[] ClosedTrades { get; set; }

        [BsonElement("balanceHistory")]
        public BalanceHistoryMongo[] BalanceHistory { get; set; }

        [BsonElement("idcount")]
        public long Idcount { get; set; }
    }

    public  class BalanceHistoryMongo
    {
        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }
    }

    public  class TradeMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public long? Id { get; set; }

        [BsonElement("id")]
        public long? idinfo { get; set; }

        [BsonElement("pair")]
        public string Pair { get; set; }

        [BsonElement("units")]
        public long Units { get; set; }

        [BsonElement("openDate")]
        public string OpenDate { get; set; }

        [BsonElement("closeDate")]
        public string CloseDate { get; set; }

        [BsonElement("long")]
        public bool Long { get; set; }

        [BsonElement("openPrice")]
        public double OpenPrice { get; set; }

        [BsonElement("closePrice")]
        public double ClosePrice { get; set; }

        [BsonElement("stopLoss")]
        public double? StopLoss { get; set; }

        [BsonElement("takeProfit")]
        public double? TakeProfit { get; set; }


        [BsonElement("init")]
        public bool Init { get; set; }


    }

    public  class OrderMongo
    {
        [BsonElement("expirationDate")]
        public object ExpirationDate { get; set; }

        [BsonElement("triggerprice")]
        public double Triggerprice { get; set; }

        [BsonElement("expired")]
        public bool Expired { get; set; }

        [BsonElement("above")]
        public bool Above { get; set; }

        [BsonElement("trade")]
        public TradeMongo Trade { get; set; }
    }

}
