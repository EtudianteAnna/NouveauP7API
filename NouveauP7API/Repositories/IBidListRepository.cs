﻿using NouveauP7API.Domain;

namespace NouveauP7API.Repositories
{
    public interface IBidListRepository
    {
        Task<List<BidList>> GetBidListsAsync();
        Task<BidList> GetByIdAsync(int id);
        Task AddAsync(BidList bidList);
        Task UpdateAsync(BidList bidList);
        Task DeleteAsync(int id);
    }
}