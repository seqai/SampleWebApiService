using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SampleWebApiService.DataAccess.Entities.Relations;

namespace SampleWebApiService.DataAccess.Entities
{
    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public string Location { get; set; }
        public string EventOrganizer { get; set; }
        public ICollection<CalendarEventMember> CalendarEventMembers { get; set; } = new List<CalendarEventMember>();
        public bool Deleted { get; set; }
    }
}
