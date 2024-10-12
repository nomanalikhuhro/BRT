using BRTAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace BRTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApiDbContext _context;

        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<object> _passwordHasher;

        public AccountController(ApiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<object>();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Find the user by username
                var user = await _context.AspNetUsers
                            .FirstOrDefaultAsync(u => u.UserName == request.Username);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                // Verify the password
                bool isValid = VerifyPassword(user.PasswordHash, request.Password);

                if (isValid)
                {

                    // Retrieve additional user data
                    var resdata = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == user.Id);
                    
                    if (resdata == null)
                    {
                        return NotFound("User data not found.");
                    }

                    // Return successful response with user data
                    return Ok(new
                    {
                        Message = "Login successful.",
                        User = resdata
                    });
                }
                else
                {
                    return Unauthorized("Invalid username or password.");
                }
            }
            catch (Exception ex) { 
            throw;
            }
        }

       
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public bool VerifyPassword(string hashedPassword, string plainTextPassword)
        {
            // The user object is not needed; passing null as the generic parameter
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainTextPassword);

            // Check if the result indicates a match
            return result == PasswordVerificationResult.Success;
        }
    }
}
