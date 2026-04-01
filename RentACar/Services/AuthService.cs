namespace RentACar.Services
{
    public class AuthService
    {
        public bool ValidateLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return true;
        }
    }
}