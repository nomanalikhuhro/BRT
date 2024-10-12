namespace BRT.Models
{
    public class AspNetUser
    {
        public string Id { get; set; }                      // Unique identifier for the user
        public string? UserName { get; set; }                // User's username
        public string? NormalizedUserName { get; set; }      // Normalized version of the username for searches
        public string? Email { get; set; }                   // User's email address
        public string? NormalizedEmail { get; set; }         // Normalized version of the email for searches
        public bool? EmailConfirmed { get; set; }            // Whether the user's email has been confirmed
        public string? PasswordHash { get; set; }            // Hashed password
        public string? SecurityStamp { get; set; }           // Security stamp used to validate password reset tokens, etc.
        public string? ConcurrencyStamp { get; set; }        // Used to handle concurrency conflicts
        public string? PhoneNumber { get; set; }             // User's phone number
        public bool? PhoneNumberConfirmed { get; set; }      // Whether the user's phone number has been confirmed
        public bool? TwoFactorEnabled { get; set; }          // Whether two-factor authentication is enabled for the user
        public DateTimeOffset? LockoutEnd { get; set; }     // When the user's lockout ends (null if not locked out)
        public bool? LockoutEnabled { get; set; }            // Whether the user can be locked out
        public int? AccessFailedCount { get; set; }          // The number of failed access attempts before lockout
        public bool? IsDeleted { get; set; }                 // Indicates if the user is marked as deleted
    }

}
