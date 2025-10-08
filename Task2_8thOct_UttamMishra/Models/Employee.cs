using System.ComponentModel.DataAnnotations;

namespace Task2_8thOct_UttamMishra.Models
{
    public class Employee
    {

        [Required]
        public int Id { get; set; }

        [Required, StringLength(30, MinimumLength = 3 , ErrorMessage = "Name should be minimum 3 and maximum 30 characters")]
        public required string Name { get; set; }

        public required string Department { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public required string MobileNo { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
    }
}
