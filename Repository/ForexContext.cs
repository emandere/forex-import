using MongoDB.Driver;
using Microsoft.Extensions.Options;
using forex_import.Models;
namespace forex_import.Repository
{
    public class ForexContext
    {
        private readonly IMongoDatabase _database = null;

        public ForexContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        

        public IMongoCollection<ForexSessionMongo> ForexSessions
        {
            get
            {
                return _database.GetCollection<ForexSessionMongo>("session");
            }
        }

        public IMongoCollection<ForexPriceMongo> ForexPrices
        {
            get
            {
                return _database.GetCollection<ForexPriceMongo>("forexdailyprices");
            }
        }
    }
}