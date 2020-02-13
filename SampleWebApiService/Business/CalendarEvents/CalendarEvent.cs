using System.Collections.Generic;

namespace SampleWebApiService.Business.CalendarEvents
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public string Location { get; set; }
        public string EventOrganizer { get; set; }
        public List<string> Members { get; set; } = new List<string>();
        public bool Deleted { get; set; }
    }
}
