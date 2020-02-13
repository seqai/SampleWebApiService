using LanguageExt;
using Microsoft.AspNetCore.JsonPatch;
using SampleWebApiService.Business.Errors;
using SampleWebApiService.DataAccess.Entities;
using SampleWebApiService.DataAccess.Entities.Relations;
using SampleWebApiService.DataAccess.Repositories;
using SampleWebApiService.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using static LanguageExt.Prelude;

namespace SampleWebApiService.Business.CalendarEvents
{
    public class CalendarEventsFacade
    {
        private readonly IReadRepository<DataAccess.Entities.CalendarEvent, CalendarEventFilter> _readRepository;
        private readonly IWriteRepository<DataAccess.Entities.CalendarEvent> _writeRepository;
        private readonly ILogger _logger;

        public CalendarEventsFacade(
            IReadRepository<DataAccess.Entities.CalendarEvent, CalendarEventFilter> readRepository,
            IWriteRepository<DataAccess.Entities.CalendarEvent> writeRepository,
            ILogger logger
        ) {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _logger = logger;
        }

        public EitherAsync<IBusinessError, List<CalendarEvent>> GetEvents(CalendarEventFilter filter)
        {
            return _readRepository.QueryAsync(filter)
                .Map(xs => xs.Map(MapEntity))
                .Match(
                    Right<IBusinessError, IEnumerable<CalendarEvent>>,
                    LogServerError<IEnumerable<CalendarEvent>>
                ).ToAsync().Map(xs => xs.ToList());
        }


        public EitherAsync<IBusinessError, CalendarEvent> GetEvent(int id)
        {
            return _readRepository.GetByIdAsync(id)
                .Map(MapEntity)
                .Match(
                    Right<IBusinessError, CalendarEvent>,
                    NoSuchItemError<CalendarEvent>(id),
                    LogServerError<CalendarEvent>
                ).ToAsync();
        }


        public EitherAsync<IBusinessError, CalendarEvent> CreateEvent(CalendarEvent calendarEvent) =>
            _writeRepository.CreateAsync(MapToEntity(calendarEvent))
                .Map(MapEntity)
                .Match(
                    Right<IBusinessError, CalendarEvent>,
                    LogServerError<CalendarEvent>
                ).ToAsync();

        public EitherAsync<IBusinessError, CalendarEvent> UpdateEvent(int id, CalendarEvent calendarEvent) =>
            _writeRepository.UpdateAsync(id, MapToEntity(calendarEvent))
                .Map(MapEntity)
                .Match(
                    Right<IBusinessError, CalendarEvent>,
                    NoSuchItemError<CalendarEvent>(id),
                    LogServerError<CalendarEvent>
                ).ToAsync();

        public EitherAsync<IBusinessError, CalendarEvent> ApplyPatchToEvent(int id, JsonPatchDocument<CalendarEvent> patchModel) =>
                GetEvent(id)
                .Map(model =>
                { 
                    patchModel.ApplyTo(model);
                    return model;
                })
                .Bind(model => UpdateEvent(id, model));

        public EitherAsync<IBusinessError, bool> DeleteEvent(int id) =>
            _writeRepository.DeleteByIdAsync(id)
                .Match(
                    Right<IBusinessError, bool>,
                    LogServerError<bool>
                ).ToAsync();

        private Either<IBusinessError, T> LogServerError<T>(Exception exception)
        {
            _logger.Error(exception, "Repository exception");
            return Left<IBusinessError, T>(new ServerError());
        }

        private static Func<Either<IBusinessError, T>> NoSuchItemError<T>(int id) =>
            () => Left<IBusinessError, T>(new NoSuchItemError($"No item with id: {id}"));

        private static DataAccess.Entities.CalendarEvent MapToEntity(CalendarEvent calendarEvent) =>
            new DataAccess.Entities.CalendarEvent
            {
                Id = calendarEvent.Id,
                Name = calendarEvent.Name,
                EventOrganizer = calendarEvent.EventOrganizer,
                Location = calendarEvent.Location,
                CalendarEventMembers = calendarEvent.Members.Map(x => new CalendarEventMember
                {
                    Member = new Member { Name = x  }
                }).ToList(),
                Time = calendarEvent.Time
            };

        private static  CalendarEvent MapEntity(DataAccess.Entities.CalendarEvent entity) =>
            new CalendarEvent
            {
                Id = entity.Id,
                Name = entity.Name,
                EventOrganizer = entity.EventOrganizer,
                Location = entity.Location,
                Members = entity.CalendarEventMembers.Map(x => x.Member.Name).ToList(),
                Time = entity.Time
            };
    }
}
