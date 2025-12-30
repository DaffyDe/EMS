using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Result
    {
        
        public int Id { get; set; }
        
        public  int StudentId { get; set; }

        public int ClassId { get; set; }

        public required string ExamTitle { get; set; }

        public int Marks { get; set; }



        [ForeignKey("ClassId")]
        public Class? Class { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

    }
}
