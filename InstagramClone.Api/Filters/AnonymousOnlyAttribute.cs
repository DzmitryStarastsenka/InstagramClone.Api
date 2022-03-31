using InstagramClone.Application.Validations.Users.ErrorMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InstagramClone.Api.Filters
{
    public class AnonymousOnlyAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Attribute blocks authrorized users to run actions of controller.
        /// </summary>
        public AnonymousOnlyAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new ObjectResult(ValidateAuthJwtTokenCommandErrorMessages.DontHaveAccessToEndpoint)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }

            return;
        }
    }
}