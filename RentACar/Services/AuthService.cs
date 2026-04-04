using RentACar.Data;
using RentACar.Models;
using System.Security.Cryptography;
using System.Text;

namespace RentACar.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User? ValidateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            return _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);
        }

        public bool RegisterUser(User user)
        {
            bool exists = _context.Users.Any(u => u.Username == user.Username);
            if (exists)
            {
                return false;
            }

            user.Password = HashPassword(user.Password);

            if (string.IsNullOrWhiteSpace(user.Role))
            {
                user.Role = "User";
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public static string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}