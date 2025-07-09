using BtgFundsApi.Models;
using BtgFundsApi.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BtgFundsApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMongoCollection<Transaction> _transactionsCollection;

        public TransactionService(IOptions<MongoDBSettings> mongoSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _transactionsCollection = database.GetCollection<Transaction>("transactions");
        }

        public async Task CreateAsync(Transaction transaction) =>
            await _transactionsCollection.InsertOneAsync(transaction);

        public async Task<List<Transaction>> GetByUserIdAsync(string userId) =>
            await _transactionsCollection.Find(t => t.UserId == userId).ToListAsync();
    }
}
