namespace WeightPlateBarbellRackingCalculator.Models;

public class PlatesCalculator
{
    private readonly IEnumerable<Plate> _allPlatesAvaiable = new List<Plate>()
        {
            new(45),
            new(35),
            new(25),
            new(10),
            new(5),
            new(2.5)
        };
    public PlatesCalculator()
    {
    }

    public RackPlates Calculate(CalculationOptions options)
    {
        return Calculate(options.TotalWeight, options.BarWeight, options.Plates, options.WeightPercentage / 100, options.Rounding);
    }

    public RackPlates Calculate(double totalWeight, double barWeight, double weightPercentage = PlatesCalculatorConstants.FULL_WEIGHT_PERCENTAGE, double rounding = PlatesCalculatorConstants.NO_ROUNDING)
    {
        return Calculate(totalWeight, barWeight, _allPlatesAvaiable, weightPercentage, rounding);
    }

    public RackPlates Calculate(double totalWeight, double barWeight, IEnumerable<Plate> plates, double weightPercentage, double rounding)
    {
        IEnumerable<Plate> availablePlates;        

        if(totalWeight <= 0 || barWeight <= 0 || totalWeight <= barWeight)
        {
            return new RackPlates();
        }

        if (plates?.Any() ?? false)
        {
            availablePlates = plates.OrderByDescending(p => p.Weight);
        }
        else
        {
            availablePlates = _allPlatesAvaiable;
        }

        var percentage = 0 <= weightPercentage && weightPercentage <= PlatesCalculatorConstants.FULL_WEIGHT_PERCENTAGE ? weightPercentage : PlatesCalculatorConstants.FULL_WEIGHT_PERCENTAGE;
        var roundingMultiple = rounding > PlatesCalculatorConstants.NO_ROUNDING ? rounding : PlatesCalculatorConstants.NO_ROUNDING;

        var weightLeft = (Math.Round(totalWeight * percentage / roundingMultiple) * roundingMultiple - barWeight) / 2.0;

        var rackingPlates = new List<Plate>();

        foreach (var plate in availablePlates)
        {
            if (plate.Quantity > 0 && weightLeft >= plate.Weight)
            {
                var numberOfPlates = Math.Floor(weightLeft / plate.Weight);

                if (numberOfPlates <= plate.Quantity)
                {
                    weightLeft %= plate.Weight;
                }
                else
                {
                    weightLeft = weightLeft % plate.Weight + plate.Weight * (numberOfPlates - plate.Quantity);
                    numberOfPlates = plate.Quantity;
                }

                rackingPlates.Add(new(plate.Weight, (int)numberOfPlates));
            }
        }

        return new(rackingPlates, weightLeft);
    }
}
