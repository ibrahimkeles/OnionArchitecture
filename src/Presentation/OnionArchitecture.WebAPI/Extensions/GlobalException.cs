using FluentValidation;
using FluentValidation.Results;
using System.Net;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.WebAPI.Extensions
{
    public class GlobalException
    {
        private RequestDelegate _next;

        public GlobalException(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }
        private Task HandleExceptionAsync(HttpContext httpContext, Exception exeption)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = exeption.Message;
            if (exeption.InnerException != null)
                message += exeption.InnerException.ToString();

            IEnumerable<ValidationFailure> errors;
            if (exeption.GetType() == typeof(ValidationException))
            {
                message = "";
                errors = ((ValidationException)exeption).Errors;
                foreach (var error in errors)
                    message += error.ErrorMessage + Environment.NewLine;

                httpContext.Response.StatusCode = 999;
                return httpContext.Response.WriteAsync(new Result(false, message, "999", errors).SerializeObject());
            }
            return httpContext.Response.WriteAsync(new Result(false, message, "500").SerializeObject());
        }

    }
}
