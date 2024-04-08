using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventByIdUseCase
    {
        public ResponseEventJson Execute(Guid id)
        {
            var dbContext = new PassInDbContext();

            /*One way to do it is: var entity = dbContext.Events.FirstOrDefault(ev => ev.Id == id);
            with the lambda function I'm telling it to go thorugh each event 
            and get me the event "ev" wwhose id is the same as the id I'm passing as a parameter*/

            //it'll know what to find because I set the Id as a PK in the db
            var entity = dbContext.Events.Include(ev => ev.Attendees).FirstOrDefault(ev => ev.Id == id);
            if (entity is null)
            {
                throw new NotFoundException("There are no events with such Id");
            }

            return new ResponseEventJson
            {
                Id = entity.Id,
                Title = entity.Title,
                Details = entity.Details,
                MaximumAttendees = entity.Maximum_Attendees,
                AttendeesAmount = entity.Attendees.Count(),
            };
        }
    }
}
