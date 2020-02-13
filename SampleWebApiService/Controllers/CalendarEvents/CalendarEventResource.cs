using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApiService.Controllers.CalendarEvents
{
    public class CalendarEventResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public string Location { get; set; }
        public string EventOrganizer { get; set; }
        public List<string> Members { get; set; } = new List<string>();

    }
}
