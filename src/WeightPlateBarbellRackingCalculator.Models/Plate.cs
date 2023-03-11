namespace WeightPlateBarbellRackingCalculator.Models;

public record struct Plate(double Weight, int Quantity = int.MaxValue)
{

    public static implicit operator (double Weight, int Quantity)(Plate value)
    {
        return (value.Weight, value.Quantity);
    }

    public static implicit operator Plate((double Weight, int Quantity) value)
    {
        return new Plate(value.Weight, value.Quantity);
    }
}
