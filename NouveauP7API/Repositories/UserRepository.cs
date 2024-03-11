using Microsoft.EntityFrameworkCore;
using NouveauP7API.Data;
using NouveauP7API.Domain;


namespace NouveauP7API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LocalDbContext _context;

        public UserRepository()

        {

        }

        public UserRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
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

        public async Task DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)

            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }
        // Méthode pour ajouter un utilisateur  contenant le nom d'utilisateur, l'email et le mot de passe
        public async Task AddUserAsync((string Username, string Email, string Password) newUser)
        {
            var user = new User
            {

               Id = Guid.NewGuid().ToString(), // Génération d'un nouvel ID
                UserName = newUser.Username,
                PasswordHash = newUser.Password,
                Fullname = newUser.Username // Mettez le nom d'utilisateur ici ou passez-le en paramètre
                };
            await AddAsync(user);
        }

        public async Task<User?> GetUserByCredentialsAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}









