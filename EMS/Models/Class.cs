using System.ComponentModel.DataAnnotations;

namespace EMS.Models
{
    public class Class
    {
        [Key]
        public int id { get; set; }
        public int TeacherId { get; set; }  
        public Teacher Teacher { get; set; }



    }
}
