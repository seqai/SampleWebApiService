using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LanguageExt;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SampleWebApiService.DataAccess.Entities;
using SampleWebApiService.DataAccess.Entities.Relations;
using SampleWebApiService.Filters;
using static LanguageExt.Prelude;

namespace SampleWebApiService.DataAccess.Repositories
{
    public class CalendarEventRepository : IReadRepository<CalendarEvent, CalendarEventFilter>, IWriteRepository<CalendarEvent>
    {
        private readonly ServiceDbContext _context;

        public CalendarEventRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public TryOptionAsync<CalendarEvent> GetByIdAsync(int id) => async () =>
            await _context.CalendarEvents
                .Include(x => x.CalendarEventMembers)
                .ThenInclude(x => x.Member)
                .FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);

        public TryAsync<IList<CalendarEvent>> QueryAsync(CalendarEventFilter filter) => async () =>
            await ApplyFilter(
                    _context.CalendarEvents
                        .Include(x => x.CalendarEventMembers)
                        .ThenInclude(x => x.Member),
                    filter
                ).ToListAsync();

        public TryAsync<CalendarEvent> CreateAsync(CalendarEvent entity) => async () =>
        {
            entity.CalendarEventMembers = await GetMembersAsync(entity);
            
            var created = _context.Add(entity);
            await _context.SaveChangesAsync();
            
            return created.Entity;
        };

        public TryOptionAsync<CalendarEvent> UpdateAsync(int id, CalendarEvent updatedEntity) =>
            GetByIdAsync(id).MapAsync(async existingEntity =>
            {

                existingEntity.CalendarEventMembers = await GetMembersAsync(updatedEntity);

                existingEntity.Name = updatedEntity.Name;
                existingEntity.EventOrganizer = updatedEntity.EventOrganizer;
                existingEntity.Time = updatedEntity.Time;
                existingEntity.Location = updatedEntity.Location;

                _context.CalendarEvents.Update(existingEntity);
                await _context.SaveChangesAsync();

                return updatedEntity;
            });

        public TryAsync<bool> DeleteByIdAsync(int id) =>
            GetByIdAsync(id).MapAsync(async entity =>
                {
                    entity.Deleted = true;
                    await _context.SaveChangesAsync();
                    return true;
                })
                .ToTry(() => false);

        private async Task<IList<CalendarEventMember>> GetMembersAsync(CalendarEvent entity)
        {
            var predicate = PredicateBuilder.New<Member>();

            foreach (var m in entity.CalendarEventMembers)
            {
                predicate.Or(p => p.Name == m.Member.Name);
            }

            var existingMembers = await _context.Members
                .Where(predicate)
                .ToDictionaryAsync(x => x.Name, x => x);

            return entity.CalendarEventMembers.Select(x => new CalendarEventMember
            {
                CalendarEvent = entity,
                Member = existingMembers.TryGetValue(x.Member.Name, out var member) ? member : x.Member
            }).ToList();
        }
        private static IQueryable<CalendarEvent> ApplyFilter(IQueryable<CalendarEvent> query, CalendarEventFilter filter)
        {
            var filteredQuery = FilterToPredicates(filter).Aggregate(query, (q, exp) => q.Where(exp));
            
            if (filter.SortType == SortType.Disabled) return filteredQuery;

            return filter.SortType == SortType.Asc
                ? filteredQuery.OrderBy(x => x.Time)
                : filteredQuery.OrderByDescending(x => x.Time);

        }

        private static IEnumerable<Expression<Func<CalendarEvent, bool>>> FilterToPredicates(CalendarEventFilter filter)
        {
            var predicates = List(
                filter.Name.Map<Expression<Func<CalendarEvent, bool>>>(name => x => x.Name == name),
                filter.Location.Map<Expression<Func<CalendarEvent, bool>>>(location => x => x.Location == location),
                filter.EventOrganizer.Map<Expression<Func<CalendarEvent, bool>>>(eventOrganizer => x => x.EventOrganizer == eventOrganizer)
            );

            return predicates.Bind(option => option);
        }

    }
}