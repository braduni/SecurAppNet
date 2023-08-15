using SecurAppNet.Models;

namespace SecurAppNet.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);

        Task<User?> GetByUsernameAsync(string username);
    }
}
