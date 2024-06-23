using Backend.Models;

namespace Backend.Services
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);
        User Register(string username, string password, string firstName, string lastName, string email);
        string GenerateJwtToken(User user);
    }
}