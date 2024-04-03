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

        public async Task<RegisterUser> GetByIdAsync(int id)
        {
            return await Task.FromResult(_registerList.FirstOrDefault(r => r.Id == id));
        }

        public async Task AddAsync(RegisterUser model)
        {
            _registerList.Add(model);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(RegisterUser model)
        {
            var existingModel = await GetByIdAsync(model.Id);
            if (existingModel != null)
            {
                existingModel.UserName = model.UserName;
                existingModel.Password = model.Password;
                existingModel.Email = model.Email;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var modelToRemove = await GetByIdAsync(id);
            if (modelToRemove != null)
            {
                _registerList.Remove(modelToRemove);
            }
            await Task.CompletedTask;
        }
    }
}
