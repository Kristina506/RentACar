using RentACar.Data;
using RentACar.Models;
using Microsoft.EntityFrameworkCore;

namespace RentACar.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await context.Users
                .Include(u => u.Reservations)
                .ThenInclude(r => r.Car)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await context.Users.FindAsync(user.Id);

            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.EGN = user.EGN;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Role = user.Role;

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}