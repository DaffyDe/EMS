using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class ClassStudent
    {
        [Key]
        public int Id { get; set; } 
        public int ClassId { get; set; }    
        public int StudentId { get; set; }

        //Forign Key Relationships

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }


        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
     
}
}
