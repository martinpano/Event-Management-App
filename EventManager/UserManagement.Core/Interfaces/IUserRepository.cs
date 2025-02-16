using UserManagement.Core.Entities;

namespace UserManagement.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
