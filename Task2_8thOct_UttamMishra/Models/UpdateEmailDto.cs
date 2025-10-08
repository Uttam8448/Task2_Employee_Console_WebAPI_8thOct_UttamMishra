using System.ComponentModel.DataAnnotations;

namespace Task2_8thOct_UttamMishra.Models
{
    public class UpdateEmailDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
    }
}
