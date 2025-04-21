using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using System.Linq;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FarmDbContext _context;

        public UserRL(FarmDbContext context)
        {
            _context = context;
        }

        public bool RegisterUser(UsersEntity user)
        {
            try
            {
                // Check if the email already exists
                var existingUser = _context.Set<UsersEntity>().FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return false; // User with this email already exists
                }

                // Add the new user
                _context.Set<UsersEntity>().Add(user);
                _context.SaveChanges();
                return true; // User successfully registered
            }
            catch (Exception)
            {
                return false; // Handle exception (e.g., database issues)
            }
        }

        public UsersEntity LoginUser(string email)
        {
            try
            {
                // Retrieve the user by email
                var user = _context.Set<UsersEntity>().FirstOrDefault(u => u.Email == email);
                return user; // Return the user (or null if not found)
            }
            catch (Exception)
            {
                return null; // Handle exception (e.g., database issues)
            }
        }
    }
}
