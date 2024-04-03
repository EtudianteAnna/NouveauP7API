using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public interface IRegisterRepository
    {
        Task<IEnumerable<RegisterUser>> GetAllAsync();
        Task<RegisterUser> GetByIdAsync(int id);
        Task AddAsync(RegisterUser model);
        Task UpdateAsync(RegisterUser model);
        Task DeleteAsync(int id);
    }
}
