using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventsId
{
    public class GetAllAttendeesByEventsIdUseCase
    {
        private readonly PassInDbContext _dbContext;

        public GetAllAttendeesByEventsIdUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public ResponseAllAttendeesJson Execute(Guid eventId)
        {
            var entity = _dbContext.Events.Include(ev => ev.Attendees).ThenInclude(attendee => attendee.CheckIn)
                .FirstOrDefault(ev => ev.Id == eventId);
            if (entity is null)
            {
                throw new NotFoundException("There are no events with such Id");
            }

            return new ResponseAllAttendeesJson
            {
                Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
                {
                    Id = attendee.Id,
                    Name = attendee.Name,
                    Email = attendee.Email,
                    CreatedAt = attendee.Created_At,
                    /*? in a class means it can sometimes be null
                    in a variable it is like an if. If it's null, return null to CheckedInAt
                    if it is not, proceed*/
                    CheckedInAt = attendee.CheckIn?.Created_at,
                }).ToList()
            };
        }
    }
}
