using System;
using LanguageExt;
using static LanguageExt.Prelude;
namespace SampleWebApiService.Filters
{
    public class CalendarEventFilter
    {
        public CalendarEventFilter(
            string name,
            string location,
            string eventOrganizer,
            SortType sortType
        ) {
            Name = ProcessString(name);
            Location = ProcessString(location);
            EventOrganizer = ProcessString(eventOrganizer);
            SortType = sortType;
        }

        public Option<string> Name { get; }
        public Option<string> Location { get; }
        public Option<string> EventOrganizer { get; }
        public SortType SortType { get; }

        private static Option<string> ProcessString(string s) => string.IsNullOrEmpty(s) ? Option<string>.None : Some(s);
    }
}
