using Microsoft.EntityFrameworkCore;
using NouveauP7API.Data;
using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {
        private readonly LocalDbContext _context;
        public RuleNameRepository(LocalDbContext context)

        {
            _context = context;
        }

        public async Task<IEnumerable<RuleName>> GetAllAsync()
        {

            return await _context.RuleName.ToListAsync();
        }
            public async Task<RuleName> GetByIdAsync(int id)
            {
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
                return await _context.RuleName.FindAsync(id);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
            }

            public async Task AddAsync(RuleName ruleName)
            {
                _context.RuleName.Add(ruleName);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(RuleName ruleName)
            {
                _context.RuleName.Update(ruleName);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var ruleName = await _context.RuleName.FindAsync(id);
                if (ruleName != null)
                {
                    _context.RuleName.Remove(ruleName);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

    
