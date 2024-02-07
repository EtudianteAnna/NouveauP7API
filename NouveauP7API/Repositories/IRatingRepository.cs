using NouveauP7API.Domain;

public interface IRatingRepository
{
    Task<IEnumerable<Rating>> GetAllAsync();
    Task AddAsync(Rating rating);
    Task UpdateAsync(Rating rating);
    Task DeleteAsync(int id);
}
