using CarGarageAPI.Data;
using CarGarageAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarGarageAPI.Controllers.Filters.ActionFilters
{
    public class Car_ValidateCarUpdateFilterAttribute:ActionFilterAttribute
    {
        private readonly ApplicationDbContext DbContext;

        public Car_ValidateCarUpdateFilterAttribute(ApplicationDbContext context)
        {
            this.DbContext = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var Vin = context.ActionArguments["Vin"] as int?;
            var Car = context.ActionArguments["Car"] as Car;
            if (Car == null)
            {
                context.ModelState.AddModelError("Car","Car can't be an empty object");
                var ProblemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(ProblemDetails);
            }
            if( Vin != Car.Vin)
            {
                context.ModelState.AddModelError("Vin", "Vin doesn't match with the passed Update object");
                var ProblemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(ProblemDetails);
            }
        }
    }
}
