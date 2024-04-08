using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventsUseCase
    {
        public ResponseRegisteredJson Execute(RequestEventJson request)
        {
            Validate(request);

            var dbContext = new PassInDbContext();

            var entity = new Infrastructure.Entities.Event
            {
                 Title = request.Title,
                 Details = request.Details,
                 Maximum_Attendees = request.MaximumAttendees,
                 Slug = request.Title.ToLower().Replace(" ", "-"),
                 //you dont the Id because it already has a .NewGuid() in its class
            };

            dbContext.Events.Add(entity);//insert into
            dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = entity.Id
            };

        }

        private void Validate(RequestEventJson request)
        {
            if(request.MaximumAttendees <= 0)
            {
                throw new ErrorOnValidationException("The number of Maximum Attendees must be greater than zero.");
            }

            if(string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ErrorOnValidationException("The Event must have a Title.");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ErrorOnValidationException("The Details section must be filled.");
            }

        }
    }
}
