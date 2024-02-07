using Microsoft.EntityFrameworkCore;
using NouveauP7API.Data;
using NouveauP7API.Domain;


namespace NouveauP7API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LocalDbContext _context;
        private object await_dbContext;


        public UserRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)

            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }
        public async Task AddUser(User user)

        {
            // Ajoutez la logique pour ajouter un utilisateur à la base de données
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByCredentialsAsync(string userName)
        {
           return  await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public Task AddUser((string Username, string Email, string Password) newUser)
        {
            throw new NotImplementedException();
        }
    }
}


    






