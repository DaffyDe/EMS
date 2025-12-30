using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Announcement
    {

        public int Id { get; set; }

        public int ClassId { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }




    }
}
