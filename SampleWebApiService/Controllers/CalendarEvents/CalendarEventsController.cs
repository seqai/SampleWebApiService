using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiService.Business.CalendarEvents;
using SampleWebApiService.Business.Errors;
using SampleWebApiService.Filters;
using Serilog;

namespace SampleWebApiService.Controllers.CalendarEvents
{
    [ApiController]
    [Route("calendar")]
    public class CalendarEventsController : ControllerBase
    {
        private readonly CalendarEventsFacade _facade;
        private readonly ILogger _logger;

        public CalendarEventsController(
            CalendarEventsFacade facade,
            ILogger logger
        )
        {
            _facade = facade;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(CalendarEventResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Get([FromQuery] CalendarEventFilterModel filterModel) =>
            _facade
                .GetEvents(MapFilter(filterModel))
                .Match(xs => Ok(xs.Map(MapToResource)), MatchError);

        [HttpGet]
        [Route("{id:min(1)}")]
        [ProducesResponseType(typeof(CalendarEventResource[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById([FromRoute] int id) =>
            _facade
                .GetEvent(id)
                .Match(x => Ok(MapToResource(x)), MatchError);

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Post([FromBody] CalendarEventModel model) =>
            _facade
                .CreateEvent(MapModel(model))
                .Match(x => Created(Url.Action(nameof(GetById), new {id = x.Id}), MapToResource(x)), MatchError);

        [HttpPut]
        [Route("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Put([FromRoute] int id, [FromBody] CalendarEventModel model) =>
            _facade
                .UpdateEvent(id, MapModel(model))
                .Match(x => NoContent(), MatchError);

        [HttpPatch]
        [Route("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<CalendarEvent> patchModel) =>
            _facade
                .ApplyPatchToEvent(id, patchModel)
                .Match(x => NoContent(), MatchError);

        [HttpDelete]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Delete([FromRoute] int id) =>
            _facade.DeleteEvent(id)
                .Match(x => NoContent(), MatchError);

        private IActionResult MatchError(IBusinessError error) => error switch
            {
                NoSuchItemError e => NotFound(e.Message),
                _ => StatusCode(500) as IActionResult
            };

        private static CalendarEventResource MapToResource(CalendarEvent calendarEvent) => new CalendarEventResource
        {
            Id = calendarEvent.Id,
            Name = calendarEvent.Name,
            EventOrganizer = calendarEvent.EventOrganizer,
            Location = calendarEvent.Location,
            Members = calendarEvent.Members.ToList(),
            Time = calendarEvent.Time
        };

        private static CalendarEvent MapModel(CalendarEventModel model) => new CalendarEvent
        {
            Name = model.Name,
            EventOrganizer = model.EventOrganizer,
            Location = model.Location,
            Members = model.Members.ToList(),
            Time = model.Time
        };

        private static CalendarEventFilter MapFilter(CalendarEventFilterModel filterModel) => new CalendarEventFilter
        (
            filterModel.Name,
            filterModel.Location,
            filterModel.EventOrganizer,
            filterModel.SortType
        );

    }
}
