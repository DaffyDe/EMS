using System.ComponentModel.DataAnnotations;

namespace EMS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public string? PasswordHash { get; set; }
        [Required]
        public required string Role { get; set; }

    }
}
