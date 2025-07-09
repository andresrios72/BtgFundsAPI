using BtgFundsApi.Models;
using BtgFundsApi.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BtgFundsApi.Services
{
    public class FundService : IFundService
    {
        private readonly IMongoCollection<Fund> _fundsCollection;

        public FundService(IOptions<MongoDBSettings> mongoSettings, IMongoClient mongoClient)
        {
            // Obtiene la base de datos con el nombre configurado
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);

            // Obtiene la colección "funds"
            _fundsCollection = database.GetCollection<Fund>("funds");
        }

        // Obtiene todos los fondos disponibles
        public virtual Task<List<Fund>> GetAsync() => 
            _fundsCollection.Find(_ => true).ToListAsync();
    }
}
