using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;

namespace RentACar.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext context;

        public AuthService(ApplicationDbContext context)
        {
            this.context = context;
        }

      //  public async Task<User?> LoginAsync(string username, string password)
       // {
           // return await context.Users
          //      .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
       // }

        public async Task<bool> RegisterAsync(User user)
        {
           // bool exists = await context.Users.AnyAsync(u =>
              //  u.Username == user.Username ||
              //  u.Email == user.Email ||
              //  u.EGN == user.EGN);

           // if (exists)
            {
                return false;
            }

           // if (string.IsNullOrWhiteSpace(user.Role))
            {
              //  user.Role = "User";
            }

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return true;
        }
    }
}