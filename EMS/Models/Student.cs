using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Student
    {
        [Key]
        public int id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(100,ErrorMessage ="Name cannot excees 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "PhoneNumber is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(10, ErrorMessage = "Phone Number must be 10 digits")]
        public string PhoneNumber { get; set; }


        [ForeignKey("UserId")]
        public User? User { get; set; }

    }
}
