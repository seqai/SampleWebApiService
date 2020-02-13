using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SampleWebApiService.DataAccess.Entities.Relations;

namespace SampleWebApiService.DataAccess.Entities
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CalendarEventMember> CalendarEventMembers { get; set; } = new List<CalendarEventMember>();
    }
}
