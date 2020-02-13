namespace SampleWebApiService.DataAccess.Entities.Relations
{
    public class CalendarEventMember
    {
        public int CalendarEventId { get; set; }
        public CalendarEvent CalendarEvent { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
