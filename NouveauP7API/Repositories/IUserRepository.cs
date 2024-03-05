using NouveauP7API.Domain;

namespace NouveauP7API.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
        Task<User?> GetUserByCredentialsAsync(string username);
        Task AddUserAsync((string Username, string Email, string Password) newUser);
        Task<User?> GetByIdAsync(string id);

    }
}