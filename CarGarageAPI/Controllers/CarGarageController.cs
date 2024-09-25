using CarGarageAPI.Controllers.Filters;
using CarGarageAPI.Controllers.Filters.ActionFilters;
using CarGarageAPI.Data;
using CarGarageAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarGarageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarGarageController:ControllerBase
    {
        private readonly ApplicationDbContext _DbContext;
        public CarGarageController(ApplicationDbContext DbContext)
        {
            this._DbContext = DbContext;
        }

        [HttpGet]
        [TypeFilter(typeof(Car_QueryParamActionFilter))]
        public IActionResult GetCars([FromQuery]CarFilter Filter)
        {
            if (Filter == null)
            {
                return Ok(_DbContext.Cars.ToList());
            }
            var cars = HttpContext.Items["cars"] as IQueryable<Car>;
            var result = cars.ToList();
            return Ok(result);
        }

        [HttpGet("{Vin}")]
        [TypeFilter(typeof(Car_ValidateCarVinFilterAttribute))]
        public IActionResult GetCarByVIN(int Vin)
        {
            return Ok(HttpContext.Items["Car"]);
        }

        [HttpPost]
        public IActionResult AddCar([FromBody]Car car)
        {
            this._DbContext.Cars.Add(car);
            this._DbContext.SaveChanges();
            return CreatedAtAction(nameof(GetCarByVIN), new { Vin = car.Vin }, car);
        }

        [HttpPut("{Vin}")]
        [TypeFilter(typeof(Car_ValidateCarVinFilterAttribute))]
        [TypeFilter(typeof(Car_ValidateCarUpdateFilterAttribute))]
        public IActionResult UpdateCar(int Vin, Car car)
        {
            ////validate car
            var lCar = HttpContext.Items["Car"] as Car;
            //if (Vin != car.Vin) { return BadRequest(); }
            //if (car == null) { return BadRequest(); }
            lCar.ModelName = car.ModelName;
            lCar.BrandName = car.BrandName;
            lCar.Price = car.Price;
            lCar.Colour = car.Colour;

            _DbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{Vin}")]
        [TypeFilter(typeof(Car_ValidateCarVinFilterAttribute))]
        public IActionResult DeleteCar(int Vin)
        {
            ////validate car
            //var lCar = _DbContext.Cars.FirstOrDefault(c => c.Vin == Vin);
            //if (lCar == null) { return NotFound(); }
            var car = HttpContext.Items["Car"] as Car; 
            _DbContext.Cars.Remove(car);
            _DbContext.SaveChanges();
            return Ok();
        }
    }
}
