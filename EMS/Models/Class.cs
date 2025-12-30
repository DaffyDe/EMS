using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }


        public int TeacherId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Grade cannot exceeds be empty")]
        public string?  Grade { get; set; }  
        
        [Required(ErrorMessage = "Place cannot exceeds be empty")]
        [StringLength(100)]
        public string? Place { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        public ICollection<ClassStudent>? ClassStudents { get; set; } = new List<ClassStudent>();
        public ICollection<Result>? Results { get; set; } = new List<Result>();
        public ICollection<Attendence>? Attendences { get; set; } = new List<Attendence>();
        public ICollection<Announcement>? Announcements { get; set; } = new List<Announcement>();






    }
}
