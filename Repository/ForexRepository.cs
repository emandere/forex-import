using System.Collections.Generic;
using System.Threading.Tasks;
using forex_import.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace forex_import.Repository
{
    public class ForexRepository : IForexRepository
    {
        private readonly ForexContext _context = null;

        public ForexRepository(IOptions<Settings> settings)
        {
            _context = new ForexContext(settings);
        }


        public async Task<IEnumerable<ForexSessionMongo>> GetForexSessions(string experimentId)
        {
            var result = await _context.ForexSessions.Find((s)=>s.ExperimentId==experimentId).ToListAsync();
            return result;
        }

         public async Task<IEnumerable<ForexSessionMongo>> GetForexSessions()
        {
            var result = await _context.ForexSessions.Find(_=>true).ToListAsync();
            return result;
        }

    }    
}