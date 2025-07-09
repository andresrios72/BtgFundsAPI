using BtgFundsApi.Models;

namespace BtgFundsApi.Services
{
    public interface IFundService
    {
        Task<List<Fund>> GetAsync();
        // Si deseas, aquí puedes agregar otros métodos de FundService a futuro.
    }
}
