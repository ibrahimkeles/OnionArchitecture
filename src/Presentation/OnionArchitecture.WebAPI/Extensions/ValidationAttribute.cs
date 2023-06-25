using Microsoft.AspNetCore.Mvc.Filters;
using OnionArchitecture.Application.Utils.Validation;

namespace OnionArchitecture.WebAPI.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidationAttribute : ActionFilterAttribute
    {
        private Type validatorType;

        public ValidationAttribute(Type validatorType)
        {
            this.validatorType = validatorType;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidationTool.Validate(this.validatorType, context.ActionArguments.Values.ToArray());
            base.OnActionExecuting(context);
        }
    }
}
