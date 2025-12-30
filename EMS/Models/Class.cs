using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Class
    {
        [Key]
        public int id { get; set; }


        public int TeacherId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Grade cannot exceeds be empty")]
        public string Grade { get; set; }  
        
        [Required(ErrorMessage = "Place cannot exceeds be empty")]
        [StringLength(100)]
        public string Place { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }





    }
}
