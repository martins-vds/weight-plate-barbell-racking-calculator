namespace WeightPlateBarbellRackingCalculator.Models;

public class PlatesCalculator
{
    private const int FULL_WEIGHT_PERCENTAGE = 1;
    private const int NO_ROUNDING = 1;

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

    public RackPlates Calculate(double totalWeight, double barWeight)
    {
        return Calculate(totalWeight, barWeight, _allPlatesAvaiable);
    }

    public RackPlates Calculate(double totalWeight, double barWeight, IEnumerable<Plate> plates, double weightPercentage = FULL_WEIGHT_PERCENTAGE, double rounding = NO_ROUNDING)
    {
        IEnumerable<Plate> availablePlates;

        if (plates?.Any() ?? false)
        {
            availablePlates = plates.OrderByDescending(p => p.Weight);
        }
        else
        {
            availablePlates = _allPlatesAvaiable;
        }

        var percentage = 0 <= weightPercentage && weightPercentage <= FULL_WEIGHT_PERCENTAGE ? weightPercentage : FULL_WEIGHT_PERCENTAGE;
        var roundingMultiple = rounding > NO_ROUNDING ? rounding : NO_ROUNDING;

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
