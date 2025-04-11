using System.ComponentModel.DataAnnotations;

namespace Firebase_Auth.Data.Models.Authentication.DTO
{
    public class LoginRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        [MinLength(8)]
        public required string Password { get; set; }
    }
}
