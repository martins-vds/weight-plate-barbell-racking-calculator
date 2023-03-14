using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeightPlateBarbellRackingCalculator.Models
{
    public class CalculationOptions
    {
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Total Weight")]
        public double TotalWeight { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Bar Weight")]
        public double BarWeight { get; set; }
        public IEnumerable<Plate>? Plates { get; set; }
        [Required]
        [Range(1, 100)]
        [Display(Name = "Weight Percentage")]
        public double WeightPercentage { get; set; } = 100;
        [Range(1, 100)]
        [Display(Name = "Rounding")]
        public double Rounding { get; set; } = PlatesCalculatorConstants.NO_ROUNDING;
    }
}