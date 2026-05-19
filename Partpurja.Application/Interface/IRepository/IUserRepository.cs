using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<Role> CreateRoleAsync(string roleName);
        Task<User> CreateUserWithCustomerAsync(User user, Customer customer);
        Task<User> CreateUserWithStaffProfileAsync(User user, StaffProfile staffProfile);
    }
}
