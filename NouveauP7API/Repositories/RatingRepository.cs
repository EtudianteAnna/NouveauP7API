using Microsoft.EntityFrameworkCore;
using NouveauP7API.Data;
using NouveauP7API.Models;


namespace NouveauP7API.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly LocalDbContext _ratingContext;
       
        public RatingRepository(LocalDbContext _ratingcontext)
        {
            _ratingContext = _ratingcontext;
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {

            return await _ratingContext.Rating.ToListAsync();


         }

            public async Task AddAsync(Rating rating)
            {
            _ratingContext.Rating  .Add(rating);
                await _ratingContext.SaveChangesAsync();
            }

            public async Task UpdateAsync(Rating rating)
            {
                _ratingContext.Rating.Update(rating);
                await _ratingContext.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var rating = await _ratingContext.Rating.FindAsync(id);
                if (rating != null)
                {
                    _ratingContext.Rating.Remove(rating);
                    await _ratingContext.SaveChangesAsync();
                }
            }
        }
    }




