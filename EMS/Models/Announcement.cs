namespace EMS.Models
{
    public class Announcement
    {

        public int AnnouncementId { get; set; }

        public int ClassId { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DateTime { get; set; }




    }
}
