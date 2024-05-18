using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RepositoryLayer.Service
{
    public class UserRepoImpl : IUserRepo
    {
        public static String Otp;
        public static string Email;
        private readonly DapperContext _context;

        public UserRepoImpl(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> createUser(UserBody user)
        {
            string hashedPassword = HashPassword(user.Password);


            var query = @"
                INSERT INTO UserEntity (Name, Email, Password, MobileNumber)
                VALUES (@name, @email, @password, @number)";

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new
                {
                    name = user.Name,
                    email = user.Email,
                    password = hashedPassword,
                    number = user.MobileNumber
                });
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<string> Login(string email, string password, IConfiguration configuration)
        {
            try
            {
                var query = "SELECT * FROM UserEntity WHERE Email = @Email";
                using (var connection = _context.CreateConnection())
                {
                    var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });

                    // Check if user exists
                    if (user != null)
                    {
                        // Hash the provided password
                        string hashedPassword = HashPassword(password);

                        // Compare the hashed password with the stored hashed password
                        if (hashedPassword == user.Password)
                        {
                            // Generate JWT token
                            var token = GenerateJwtToken(user, configuration);
                            return token;
                        }
                    }

                    // Authentication failed
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        // Generating JWT Tokens    
        private string GenerateJwtToken(User user, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]);
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
               // new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString())
               // new Claim("UserId", user.UserId.ToString())
                    // Add more claims as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<User> GetUserByEmail(string email)
        {
            var query = "SELECT * FROM UserEntity WHERE Email = @Email";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
            }
        }

        public async Task ResetPassword(string email, string otp, string newPassword)
        {
            // Check if the provided OTP matches the stored OTP for the user's email
            if (otp != Otp || email != Email)
            {
                throw new ArgumentException("Invalid OTP or email.");
            }

            // Hash the new password
            string hashedPassword = HashPassword(newPassword);

            // Update the user's password in the database
            var query = "UPDATE UserEntity SET Password = @Password WHERE Email = @Email";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Password = hashedPassword, Email = email });
            }
        }


    }
}
