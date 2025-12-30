using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Attendence
    {
        
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        [ForeignKey("StudentId")] 
        public Student? Student { get; set; }

        [ForeignKey("ClassId")] 
        public Class? Class { get; set; }
    }
}
