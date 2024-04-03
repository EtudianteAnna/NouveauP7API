using NouveauP7API.Models;

public interface IRatingRepository
{
    Task<IEnumerable<Rating>> GetAllAsync();
    Task AddAsync(Rating rating);
    Task UpdateAsync(Rating rating);
    Task DeleteAsync(int id);
}
