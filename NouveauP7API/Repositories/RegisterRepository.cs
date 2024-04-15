using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly List<RegisterUser> _registerList;

        public RegisterRepository()
        {
            _registerList = new List<RegisterUser>();
        }

        public async Task<IEnumerable<RegisterUser>> GetAllAsync()
        {
            return await Task.FromResult(_registerList);
        }

        public async Task<RegisterUser> GetByIdAsync(string userName)
        {
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await Task.FromResult(_registerList.FirstOrDefault(r => r.UserName == userName));
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }

        public async Task AddAsync(RegisterUser model)
        {
            _registerList.Add(model);
            await Task.CompletedTask;
        }


        public async Task DeleteAsync(string userName)
        {
            var modelToRemove = await GetByUserName( userName);  
            if (modelToRemove != null)
            {
                _registerList.Remove(modelToRemove);
            }
            await Task.CompletedTask;
        }

        public Task<RegisterUser> GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
