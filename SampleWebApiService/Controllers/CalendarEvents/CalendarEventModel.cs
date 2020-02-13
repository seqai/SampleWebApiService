using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleWebApiService.Controllers.CalendarEvents
{
    public class CalendarEventModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string EventOrganizer { get; set; }
        [Required]
        [MinLength(1)]
        public List<string> Members { get; set; } = new List<string>();

    }
}
