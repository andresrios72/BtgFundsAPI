using BtgFundsApi.Models;

namespace BtgFundsApi.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task CreateAsync(User user);
        Task UpdateAsync(string id, User updatedUser);
    }
}
