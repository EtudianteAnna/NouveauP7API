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
            return await _context.Trade.ToListAsync();

        }

        public async Task<Trade> GetByIdAsync(int id)
        {
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await _context.Trade.FindAsync(id);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }

        public async Task AddAsync(Trade trade)
        {
                        await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trade trade)
        {
           
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trade = await _context.Trade.FindAsync(id);
            if (trade != null)
            {
                _context.Trade.Remove(trade);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }
    }
}
