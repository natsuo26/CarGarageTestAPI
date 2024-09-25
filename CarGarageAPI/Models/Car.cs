using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CarGarageAPI.Models
{
public class Car
{
    [Key]
    public int Vin { get; set; }

    [Required]
    public string? BrandName { get; set; }

    [Required]
    public string? ModelName { get; set; }

    [Required]
    public string? Colour { get; set; }

    public int? Price {  get; set; }

}
}
