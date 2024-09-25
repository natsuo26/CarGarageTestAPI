using CarGarageAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace CarGarageAPI.Controllers.Filters.ActionFilters
{
    public class Car_QueryParamActionFilter:ActionFilterAttribute
    {

        private readonly ApplicationDbContext DbContext;

        public Car_QueryParamActionFilter(ApplicationDbContext context)
        {
            this.DbContext = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var filter = context.ActionArguments["filter"] as CarFilter;
            if(filter != null)
            {
                var cars = DbContext.Cars.AsQueryable();
                if (!string.IsNullOrEmpty(filter.BrandName))
                {
                    cars=cars.Where(c=>c.BrandName==filter.BrandName);
                }
                if(!string.IsNullOrEmpty(filter.ModelName))
                {
                    cars=cars.Where(c=>c.ModelName==filter.ModelName);
                }
                if (!string.IsNullOrEmpty(filter.Colour))
                {
                    cars = cars.Where(c => c.Colour == filter.Colour);
                }
                if (filter.Price.HasValue)
                {
                    cars=cars.Where(c=>c.Price.Value<=filter.Price.Value);
                }
                if (!cars.Any())
                {
                    context.ModelState.AddModelError("QueryParams", "no match in the database based on given query");
                    var ProblemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(ProblemDetails);
                }
                else
                {
                    context.HttpContext.Items["cars"] = cars;
                }
            }
        }
    }
}
