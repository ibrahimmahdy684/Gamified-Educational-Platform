using System.ComponentModel.DataAnnotations;

namespace GamifiedPlatform.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Type { get; set; } // Learner, Instructor, Admin
    }
}
