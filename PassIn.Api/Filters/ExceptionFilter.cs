using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

namespace PassIn.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //it'll return a true or false
            var result = context.Exception is PassInException;
            if (result)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknownError(context);
            }

        }
        private void HandleProjectException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));

            }
            else if (context.Exception is ErrorOnValidationException)
            {
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Result = new BadRequestObjectResult(new ResponseErrorJson(context.Exception.Message));
                }
            }
            else if (context.Exception is ConflictException)
            {
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    context.Result = new ConflictObjectResult(new ResponseErrorJson(context.Exception.Message));
                }
            }


        }
        private void ThrowUnknownError(ExceptionContext context)
        {
            //int is to translate the InternalServerError, that is an enum, to an int
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson("Unknown Error"));
        }
    }
}
