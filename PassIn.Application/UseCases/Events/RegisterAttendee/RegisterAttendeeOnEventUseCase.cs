using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttendeeOnEventUseCase
    {
        private readonly PassInDbContext _dbContext;
        
        public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseRegisteredJson Execute(Guid eventId, RequestRegisterEventJson request)
        {

             Validate(eventId, request);

            var entity = new Infrastructure.Entities.Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow,
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = entity.Id,
            };

        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            //Are there any events ev that have the same Id as the eventId I received as a parameter?
            var eventEntity = _dbContext.Events.Find(eventId);
            if (eventEntity is null)
            {
                throw new NotFoundException("There are no events with such Id");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("Invalid name.");
            }

            if (EmailIsValid(request.Email) == false)
            {
                throw new ErrorOnValidationException("Invalid e-mail.");
            }

            var attendeeAlreadyRegistered = _dbContext
                .Attendees
                .Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == eventId);

            if (attendeeAlreadyRegistered == false)
            {
                throw new ConflictException("You cannot register twice on the same event.");
            }

           var attendeesForEvent = _dbContext.Attendees.Count(attendee => attendee.Event_Id == eventId);
            if(attendeesForEvent == eventEntity.Maximum_Attendees)
            {
                throw new ErrorOnValidationException("There is no room for new attendees in this event.");
            }
        }

        private bool EmailIsValid(string email)
        {
            try
            {
                new MailAddress(email);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
