using CarGarageAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarGarageAPI.Controllers.Filters.ActionFilters
{
    public class Car_ValidateCarVinFilterAttribute:ActionFilterAttribute
    {
        private readonly ApplicationDbContext DbContext;

        public Car_ValidateCarVinFilterAttribute(ApplicationDbContext context)
        {
            this.DbContext = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var CarVin = context.ActionArguments["Vin"] as int?;
            if (CarVin.HasValue)
            {
                if (CarVin.Value <= 0)
                {
                    //check if the request contains correct Vin or not.
                    context.ModelState.AddModelError("Vin", "Car Vin is invalid");
                    var ProblemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result=new BadRequestObjectResult(ProblemDetails);
                }
                else
                {
                    var Car = this.DbContext.Cars.Find(CarVin.Value);
                    //check for car if it exists in DB.
                    if (Car == null)
                    {
                        context.ModelState.AddModelError("Vin", "Car doesn't exists");
                        var ProblemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(ProblemDetails);
                    }
                    else//if it does exists save it in the context of the Http's filters to access anytime.
                    {
                        context.HttpContext.Items["Car"] = Car;
                    }
                }
            }
        }



    }
}
