using FluentAssertions;
using WeightPlateBarbellRackingCalculator.Models;

namespace WeightPlateBarbellRackingCalculator.Tests;

public class PlatesCalculatorTests
{
    [Theory]    
    [MemberData(nameof(DataWithStandardParameters))]
    public void Test1(double totalWeightLb, double barWeightLb, RackPlates exceptedRackPlates)
    {
        var calculator = new PlatesCalculator();

        var plates = calculator.Calculate(totalWeightLb, barWeightLb);

        plates.Plates.Should().BeEquivalentTo(exceptedRackPlates.Plates);
        plates.Diff.Should().Be(exceptedRackPlates.Diff);
    }

    [Theory]
    [MemberData(nameof(DataWithCustomParameters))]
    public void Test2(double totalWeightLb, double barWeightLb, Plate[] platesAvailable, double weightPercentage, double rounding, RackPlates expectedRackPlates)
    {
        var calculator = new PlatesCalculator();

        var plates = calculator.Calculate(totalWeightLb, barWeightLb, platesAvailable, weightPercentage, rounding);

        plates.Plates.Should().BeEquivalentTo(expectedRackPlates.Plates);
        plates.Diff.Should().Be(expectedRackPlates.Diff);
    }

    public static IEnumerable<object[]> DataWithStandardParameters => new List<object[]>
    {
        new object[] { 115, 45, new RackPlates(new List<Plate>() { new(Weight: 35, Quantity: 1) }, Diff: 0)},
        new object[] { 185, 45, new RackPlates(new List<Plate>() { new(Weight: 45, Quantity: 1), new(Weight: 25, Quantity: 1) }, Diff: 0)},
        new object[] { 100, 45, new RackPlates(new List<Plate>() { new(Weight: 25, Quantity: 1), new(Weight: 2.5, Quantity: 1) }, Diff: 0)}
    };

    public static IEnumerable<object[]> DataWithCustomParameters => new List<object[]>
    {
        new object[] { 185, 45, new Plate[] { new(35, 2) }, 1, 1, new RackPlates(new List<Plate>() { new(Weight: 35, Quantity: 2) }, Diff: 0) },
        new object[] { 185, 45, new Plate[] { new(35, 1), new(25, 1), new(10, 1) }, 1, 1, new RackPlates(new List<Plate>() { new(Weight: 35, Quantity: 1), new(Weight: 25, Quantity: 1), new(Weight: 10, Quantity: 1) }, Diff: 0) },
        new object[] { 160, 45, new Plate[] { new(10, 3) }, 0.4, 5, new RackPlates(new List<Plate>() { new(Weight: 10, Quantity: 1) }, Diff: 0) },
        new object[] { 160, 45, new Plate[] { new(5, 3), new(2.5, 3) }, 0.4, 1, new RackPlates(new List<Plate>() { new(Weight: 5, Quantity: 1), new(Weight: 2.5, Quantity: 1) }, Diff: 2) },
        new object[] { 160, 45, new Plate[] { new(5, 0) }, 100, 1, new RackPlates(new List<Plate>(), Diff: 57.5) }
    };
}
