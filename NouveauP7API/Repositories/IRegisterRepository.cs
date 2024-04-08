using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public interface IRegisterRepository
    {
        Task<IEnumerable<RegisterUser>> GetAllAsync();
        Task<RegisterUser> GetByUserName(string userName);
        Task AddAsync(RegisterUser model);
        Task DeleteAsync(string userName);
    }
}
