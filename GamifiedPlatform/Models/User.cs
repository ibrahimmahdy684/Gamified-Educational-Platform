using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

 namespace GamifiedPlatform.Models
{
    // This defines the User model that corresponds to the Users table
    [Table("Users")]  // Optional if your table name matches the class name
    public class User
    {
        [Key]  // Specifies that this is the primary key
        public int UserId { get; set; }  // Renamed to UserId

        [Required]  // Specifies that Name is a required field
        [StringLength(100)]  // Optional: Set a maximum length for the Name field
        public string Name { get; set; }

        [Required]  // Specifies that Email is a required field
        [StringLength(100)]  // Optional: Set a maximum length for the Email field
        [EmailAddress]  // Ensures the value is a valid email format
        public string Email { get; set; }

        [StringLength(15)]  // Optional: Set a maximum length for the Phone field
        public string Phone { get; set; }

        [StringLength(255)]  // Optional: Set a maximum length for the Address field
        public string Address { get; set; }
    }
}
