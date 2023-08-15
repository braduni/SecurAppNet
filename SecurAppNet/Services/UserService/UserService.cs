using Microsoft.EntityFrameworkCore;
using SecurAppNet.Data;
using SecurAppNet.Models;

namespace SecurAppNet.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
            
        }
        public async Task CreateAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();  
        }

        public async Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByUsernameAsync(string  username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}