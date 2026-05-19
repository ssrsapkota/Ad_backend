using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Customer)
                .Include(u => u.StaffProfile)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task<Role> CreateRoleAsync(string roleName)
        {
            var role = new Role { Name = roleName };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<User> CreateUserWithCustomerAsync(User user, Customer customer)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            customer.UserId = user.Id;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            await tx.CommitAsync();
            return user;
        }

        public async Task<User> CreateUserWithStaffProfileAsync(User user, StaffProfile staffProfile)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            staffProfile.UserId = user.Id;
            _context.StaffProfiles.Add(staffProfile);
            await _context.SaveChangesAsync();

            await tx.CommitAsync();
            return user;
        }
    }
}
