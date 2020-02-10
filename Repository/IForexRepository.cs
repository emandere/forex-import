using System.Threading.Tasks;
using System.Collections.Generic;
using forex_import.Models;

namespace forex_import.Repository
{
    public interface IForexRepository
    {
        //Task<IEnumerable<ForexExperiment>> GetAllNotes();
        Task<IEnumerable<ForexSessionMongo>> GetForexSessions();
        Task<IEnumerable<ForexSessionMongo>> GetForexSessions(string experimentId);
    }
}
