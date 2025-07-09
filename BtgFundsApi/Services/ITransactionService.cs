using BtgFundsApi.Models;

namespace BtgFundsApi.Services
{
    public interface ITransactionService
    {
        Task CreateAsync(Transaction transaction);
        Task<List<Transaction>> GetByUserIdAsync(string userId);
    }
}
