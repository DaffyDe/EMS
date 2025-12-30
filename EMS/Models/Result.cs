using System.ComponentModel.DataAnnotations;

namespace EMS.Models
{
    public class Result
    {

        
        public int ResultId { get; set; }
        
        public  int StudentId { get; set; }

        public int ClassId { get; set; }

        public required string ExamTitle { get; set; }

        public int Marks { get; set; }

    }
}
