using Microsoft.EntityFrameworkCore;
using NouveauP7API.Models;
using NouveauP7API.Data;

namespace NouveauP7API.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly LocalDbContext _context;

        public TradeRepository(LocalDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Trade>> GetAllAsync()
        {
            return await _context.Trades.ToListAsync();

        }

        public async Task<Trade> GetByIdAsync(int id)
        {
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await _context.Trades.FindAsync(id);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }

        public async Task AddAsync(Trade trade)
        {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trade trade)
        {
            _context.Trades.Update(trade);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade != null)
            {
                _context.Trades.Remove(trade);
                await _context.SaveChangesAsync();
            }
        }
    }
}
