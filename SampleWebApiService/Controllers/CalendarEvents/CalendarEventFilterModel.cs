using Microsoft.AspNetCore.Mvc;

namespace SampleWebApiService.Controllers.CalendarEvents
{
    public class CalendarEventFilterModel
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "location")]
        public string Location { get; set; }
        [FromQuery(Name = "eventOrganizer")]
        public string EventOrganizer { get; set; }
        [FromQuery(Name = "sortType")]
        public SortType SortType { get; set; } = SortType.Disabled;
    }
}
