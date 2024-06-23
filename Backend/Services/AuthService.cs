using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Backend.Models;
using Backend.Data;

namespace Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            user.LastLogin = DateTime.UtcNow;
            _context.SaveChanges();

            return user;
        }

        public User Register(string username, string password, string firstName, string lastName, string email)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                return null; // User already exists
            }

            var user = new User
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                HashedPassword = _passwordHasher.HashPassword(null, password),
                Status = "Active",
                LastLogin = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Create a bank account for the new user
            var bankAccount = new BankAccount
            {
                UserId = user.Id,
                Balance = 0 // Initial balance
            };

            _context.BankAccounts.Add(bankAccount);
            _context.SaveChanges();

            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
