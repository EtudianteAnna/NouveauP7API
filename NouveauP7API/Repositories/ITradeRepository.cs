using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public interface ITradeRepository
    {
        Task<IEnumerable<Trade>> GetAllAsync();
        Task<Trade> GetByIdAsync(int id);
        Task AddAsync(Trade trade);
        Task UpdateAsync(Trade trade);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}