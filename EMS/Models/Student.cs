using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required ]
        [StringLength(100,ErrorMessage = "Name cannot exceeds 100 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "PhoneNumber is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(10, ErrorMessage = "Phone Number must be 10 digits")]
        public required string PhoneNumber { get; set; }


        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<ClassStudent>? ClassStudents { get; set; } = new List<ClassStudent>();

        public ICollection<Attendence>? Attendences { get; set; } = new List<Attendence>();
        public ICollection<Result>? Results { get; set; } = new List<Result>();


    }
}
