using System;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// using BusinessLayer.Service;
namespace BusinessLayer.Service{
    public class UserBL:IUserBL{
        private readonly IUserRL _userRL;
        private readonly IConfiguration _configuration;
        
        public UserBL(IUserRL userRL,IConfiguration configuration){
             _userRL = userRL;
              _configuration = configuration;
        }

         public bool RegisterUser(RegisterDTO registerDTO)
        {
            // Map the RegisterDTO to UsersEntity
            var userEntity = new UsersEntity
            {
                
                // Hash passwords in real-world scenarios
                FullName = registerDTO.FullName,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                Role = registerDTO.Role
            };

            // Register the user using IUserRL
            bool isRegistered = _userRL.RegisterUser(userEntity);

            // If registration is successful, generate a JWT token for the new user
            if (isRegistered)
            {
                var token = GenerateJwtToken(userEntity); // Generate token for the newly registered user
                // You can store this token in your response or return it
                // (e.g., returning it directly for now)
                Console.WriteLine($"Generated JWT Token: {token}");
            }

            return isRegistered;
        }

           public string LoginUser(LoginDTO loginDTO)
        {
            var user = _userRL.LoginUser(loginDTO.Email);

            if (user != null)
            {
                // Password comparison (Hashing should be done for security in real-world applications)
                if (user.Password == loginDTO.Password)
                {
                    // Create a JWT token
                    var token = GenerateJwtToken(user);
                    return token; // Return the JWT token on successful login
                }
                else
                {
                    return "Invalid credentials"; // Password mismatch
                }
            }
            else
            {
                return "User not found"; // No user found with the given email
            }
        }

          private string GenerateJwtToken(UsersEntity user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your-app", // Your app's name
                audience: "your-app-users", // Your app's users
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Generate the JWT string
        }
    }
}