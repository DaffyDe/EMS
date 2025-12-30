using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "name can't empty")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number can not empty")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 10 characters")]
        public int PhoneNumber { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<Class>? Classes { get; set; } = new List<Class>();
    }
}
