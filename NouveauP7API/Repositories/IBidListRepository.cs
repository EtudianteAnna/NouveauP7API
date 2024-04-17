using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public interface IBidListRepository
    {
        Task<List<BidList>> GetBidListsAsync();
        Task<BidList> GetByIdAsync(int Id);
        Task AddAsync(BidList bidList);
        Task UpdateAsync(BidList bidList);
        Task DeleteAsync(int Id);
    }
}