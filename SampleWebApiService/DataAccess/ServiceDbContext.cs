using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleWebApiService.DataAccess.Entities;

namespace SampleWebApiService.DataAccess
{
    public class ServiceDbContext : DbContext
    {
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<Member> Members { get; set; }

    }
}
