using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SampleWebApiService.DataAccess.Entities.Relations;
using SampleWebApiService.Infrastructure.Configuration;

namespace SampleWebApiService.DataAccess
{
    internal class ServiceSqlLiteDbContext : ServiceDbContext
    {
        private readonly string _connectionString;

        public ServiceSqlLiteDbContext(IOptions<PersistenceConfiguration> configuration)
        {
            _connectionString = configuration.Value.ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarEventMember>()
                .HasKey(cem => new { cem.CalendarEventId, cem.MemberId });

            modelBuilder.Entity<CalendarEventMember>()
                .HasOne(cem => cem.CalendarEvent)
                .WithMany(ce => ce.CalendarEventMembers)
                .HasForeignKey(cem => cem.CalendarEventId);

            modelBuilder.Entity<CalendarEventMember>()
                .HasOne(cem => cem.Member)
                .WithMany(m => m.CalendarEventMembers)
                .HasForeignKey(m => m.MemberId);
        }
    }
}
