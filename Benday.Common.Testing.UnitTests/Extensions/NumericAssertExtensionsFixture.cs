using System;
using Xunit.Abstractions;

namespace Benday.Common.Testing.UnitTests.Extensions;

public class NumericAssertExtensionsFixture : TestClassBase
{
    public NumericAssertExtensionsFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region ShouldBeGreaterThan Tests

    [Fact]
    public void ShouldBeGreaterThan_WhenGreater_ReturnsValue()
    {
        // Arrange
        var actual = 10;

        // Act
        var result = actual.ShouldBeGreaterThan(5, "10 > 5");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void ShouldBeGreaterThan_WhenEqual_Throws()
    {
        // Arrange
        var actual = 5;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeGreaterThan(5, "Should be greater"));

        Assert.Contains("Should be greater", ex.Message);
    }

    [Fact]
    public void ShouldBeGreaterThan_WhenLess_Throws()
    {
        // Arrange
        var actual = 3;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeGreaterThan(5, "Should be greater"));

        Assert.Contains("Should be greater", ex.Message);
    }

    #endregion

    #region ShouldBeGreaterThanOrEqualTo Tests

    [Fact]
    public void ShouldBeGreaterThanOrEqualTo_WhenGreater_ReturnsValue()
    {
        // Arrange
        var actual = 10;

        // Act
        var result = actual.ShouldBeGreaterThanOrEqualTo(5, "10 >= 5");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void ShouldBeGreaterThanOrEqualTo_WhenEqual_ReturnsValue()
    {
        // Arrange
        var actual = 5;

        // Act
        var result = actual.ShouldBeGreaterThanOrEqualTo(5, "5 >= 5");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void ShouldBeGreaterThanOrEqualTo_WhenLess_Throws()
    {
        // Arrange
        var actual = 3;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeGreaterThanOrEqualTo(5, "Should be >= 5"));

        Assert.Contains("Should be >= 5", ex.Message);
    }

    #endregion

    #region ShouldBeLessThan Tests

    [Fact]
    public void ShouldBeLessThan_WhenLess_ReturnsValue()
    {
        // Arrange
        var actual = 3;

        // Act
        var result = actual.ShouldBeLessThan(5, "3 < 5");

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void ShouldBeLessThan_WhenEqual_Throws()
    {
        // Arrange
        var actual = 5;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeLessThan(5, "Should be less"));

        Assert.Contains("Should be less", ex.Message);
    }

    #endregion

    #region ShouldBeLessThanOrEqualTo Tests

    [Fact]
    public void ShouldBeLessThanOrEqualTo_WhenLess_ReturnsValue()
    {
        // Arrange
        var actual = 3;

        // Act
        var result = actual.ShouldBeLessThanOrEqualTo(5, "3 <= 5");

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void ShouldBeLessThanOrEqualTo_WhenEqual_ReturnsValue()
    {
        // Arrange
        var actual = 5;

        // Act
        var result = actual.ShouldBeLessThanOrEqualTo(5, "5 <= 5");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void ShouldBeLessThanOrEqualTo_WhenGreater_Throws()
    {
        // Arrange
        var actual = 10;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeLessThanOrEqualTo(5, "Should be <= 5"));

        Assert.Contains("Should be <= 5", ex.Message);
    }

    #endregion

    #region ShouldBeInRange Tests

    [Fact]
    public void ShouldBeInRange_WhenInRange_ReturnsValue()
    {
        // Arrange
        var actual = 5;

        // Act
        var result = actual.ShouldBeInRange(1, 10, "5 is in [1,10]");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void ShouldBeInRange_WhenAtMinimum_ReturnsValue()
    {
        // Arrange
        var actual = 1;

        // Act
        var result = actual.ShouldBeInRange(1, 10, "1 is in [1,10]");

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ShouldBeInRange_WhenAtMaximum_ReturnsValue()
    {
        // Arrange
        var actual = 10;

        // Act
        var result = actual.ShouldBeInRange(1, 10, "10 is in [1,10]");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void ShouldBeInRange_WhenOutOfRange_Throws()
    {
        // Arrange
        var actual = 15;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeInRange(1, 10, "Out of range"));

        Assert.Contains("Out of range", ex.Message);
    }

    #endregion

    #region ShouldNotBeInRange Tests

    [Fact]
    public void ShouldNotBeInRange_WhenOutsideRange_ReturnsValue()
    {
        // Arrange
        var actual = 15;

        // Act
        var result = actual.ShouldNotBeInRange(1, 10, "15 not in [1,10]");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void ShouldNotBeInRange_WhenInRange_Throws()
    {
        // Arrange
        var actual = 5;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeInRange(1, 10, "Should not be in range"));

        Assert.Contains("Should not be in range", ex.Message);
    }

    #endregion

    #region ShouldBePositive Tests

    [Fact]
    public void ShouldBePositive_WhenPositive_ReturnsValue()
    {
        // Arrange
        var actual = 5;

        // Act
        var result = actual.ShouldBePositive("5 is positive");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void ShouldBePositive_WhenZero_Throws()
    {
        // Arrange
        var actual = 0;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBePositive("Should be positive"));

        Assert.Contains("Should be positive", ex.Message);
    }

    [Fact]
    public void ShouldBePositive_WhenNegative_Throws()
    {
        // Arrange
        var actual = -5;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBePositive("Should be positive"));

        Assert.Contains("Should be positive", ex.Message);
    }

    #endregion

    #region ShouldBeNegative Tests

    [Fact]
    public void ShouldBeNegative_WhenNegative_ReturnsValue()
    {
        // Arrange
        var actual = -5;

        // Act
        var result = actual.ShouldBeNegative("-5 is negative");

        // Assert
        Assert.Equal(-5, result);
    }

    [Fact]
    public void ShouldBeNegative_WhenZero_Throws()
    {
        // Arrange
        var actual = 0;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeNegative("Should be negative"));

        Assert.Contains("Should be negative", ex.Message);
    }

    [Fact]
    public void ShouldBeNegative_WhenPositive_Throws()
    {
        // Arrange
        var actual = 5;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeNegative("Should be negative"));

        Assert.Contains("Should be negative", ex.Message);
    }

    #endregion

    #region ShouldBeZero Tests

    [Fact]
    public void ShouldBeZero_WhenZero_ReturnsValue()
    {
        // Arrange
        var actual = 0;

        // Act
        var result = actual.ShouldBeZero("Is zero");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ShouldBeZero_WhenNonZero_Throws()
    {
        // Arrange
        var actual = 5;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeZero("Should be zero"));

        Assert.Contains("Should be zero", ex.Message);
    }

    #endregion

    #region ShouldNotBeZero Tests

    [Fact]
    public void ShouldNotBeZero_WhenNonZero_ReturnsValue()
    {
        // Arrange
        var actual = 5;

        // Act
        var result = actual.ShouldNotBeZero("Not zero");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void ShouldNotBeZero_WhenZero_Throws()
    {
        // Arrange
        var actual = 0;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeZero("Should not be zero"));

        Assert.Contains("Should not be zero", ex.Message);
    }

    #endregion

    #region ShouldBeApproximatelyEqualTo Tests (double)

    [Fact]
    public void ShouldBeApproximatelyEqualTo_Double_WhenWithinTolerance_ReturnsValue()
    {
        // Arrange
        var actual = 10.05;

        // Act
        var result = actual.ShouldBeApproximatelyEqualTo(10.0, 0.1, "Close enough");

        // Assert
        Assert.Equal(10.05, result);
    }

    [Fact]
    public void ShouldBeApproximatelyEqualTo_Double_WhenOutsideTolerance_Throws()
    {
        // Arrange
        var actual = 10.2;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeApproximatelyEqualTo(10.0, 0.1, "Not close enough"));

        Assert.Contains("Not close enough", ex.Message);
    }

    #endregion

    #region ShouldBeApproximatelyEqualTo Tests (float)

    [Fact]
    public void ShouldBeApproximatelyEqualTo_Float_WhenWithinTolerance_ReturnsValue()
    {
        // Arrange
        var actual = 10.05f;

        // Act
        var result = actual.ShouldBeApproximatelyEqualTo(10.0f, 0.1f, "Close enough");

        // Assert
        Assert.Equal(10.05f, result);
    }

    [Fact]
    public void ShouldBeApproximatelyEqualTo_Float_WhenOutsideTolerance_Throws()
    {
        // Arrange
        var actual = 10.2f;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeApproximatelyEqualTo(10.0f, 0.1f, "Not close enough"));

        Assert.Contains("Not close enough", ex.Message);
    }

    #endregion

    #region ShouldBeApproximatelyEqualTo Tests (decimal)

    [Fact]
    public void ShouldBeApproximatelyEqualTo_Decimal_WhenWithinTolerance_ReturnsValue()
    {
        // Arrange
        var actual = 10.05m;

        // Act
        var result = actual.ShouldBeApproximatelyEqualTo(10.0m, 0.1m, "Close enough");

        // Assert
        Assert.Equal(10.05m, result);
    }

    [Fact]
    public void ShouldBeApproximatelyEqualTo_Decimal_WhenOutsideTolerance_Throws()
    {
        // Arrange
        var actual = 10.2m;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeApproximatelyEqualTo(10.0m, 0.1m, "Not close enough"));

        Assert.Contains("Not close enough", ex.Message);
    }

    #endregion

    #region ShouldNotBeNaN Tests

    [Fact]
    public void ShouldNotBeNaN_Double_WhenNotNaN_ReturnsValue()
    {
        // Arrange
        var actual = 10.5;

        // Act
        var result = actual.ShouldNotBeNaN("Not NaN");

        // Assert
        Assert.Equal(10.5, result);
    }

    [Fact]
    public void ShouldNotBeNaN_Double_WhenNaN_Throws()
    {
        // Arrange
        var actual = double.NaN;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeNaN("Should not be NaN"));

        Assert.Contains("Should not be NaN", ex.Message);
    }

    [Fact]
    public void ShouldNotBeNaN_Float_WhenNotNaN_ReturnsValue()
    {
        // Arrange
        var actual = 10.5f;

        // Act
        var result = actual.ShouldNotBeNaN("Not NaN");

        // Assert
        Assert.Equal(10.5f, result);
    }

    [Fact]
    public void ShouldNotBeNaN_Float_WhenNaN_Throws()
    {
        // Arrange
        var actual = float.NaN;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeNaN("Should not be NaN"));

        Assert.Contains("Should not be NaN", ex.Message);
    }

    #endregion

    #region ShouldBeFinite Tests

    [Fact]
    public void ShouldBeFinite_Double_WhenFinite_ReturnsValue()
    {
        // Arrange
        var actual = 10.5;

        // Act
        var result = actual.ShouldBeFinite("Is finite");

        // Assert
        Assert.Equal(10.5, result);
    }

    [Fact]
    public void ShouldBeFinite_Double_WhenInfinity_Throws()
    {
        // Arrange
        var actual = double.PositiveInfinity;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeFinite("Should be finite"));

        Assert.Contains("Should be finite", ex.Message);
    }

    [Fact]
    public void ShouldBeFinite_Double_WhenNaN_Throws()
    {
        // Arrange
        var actual = double.NaN;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeFinite("Should be finite"));

        Assert.Contains("Should be finite", ex.Message);
    }

    [Fact]
    public void ShouldBeFinite_Float_WhenFinite_ReturnsValue()
    {
        // Arrange
        var actual = 10.5f;

        // Act
        var result = actual.ShouldBeFinite("Is finite");

        // Assert
        Assert.Equal(10.5f, result);
    }

    [Fact]
    public void ShouldBeFinite_Float_WhenInfinity_Throws()
    {
        // Arrange
        var actual = float.NegativeInfinity;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeFinite("Should be finite"));

        Assert.Contains("Should be finite", ex.Message);
    }

    #endregion

    #region Fluent Chaining Tests

    [Fact]
    public void NumericExtensions_CanBeChained()
    {
        // This demonstrates fluent chaining
        var result = 5
            .ShouldBePositive("Is positive")
            .ShouldBeGreaterThan(0, "> 0")
            .ShouldBeLessThan(10, "< 10")
            .ShouldBeInRange(1, 10, "In range [1,10]")
            .ShouldNotBeZero("Not zero");

        Assert.Equal(5, result);
    }

    [Fact]
    public void NumericExtensions_Double_CanBeChained()
    {
        // This demonstrates fluent chaining with doubles
        var result = 5.5
            .ShouldNotBeNaN("Not NaN")
            .ShouldBeFinite("Is finite")
            .ShouldBeApproximatelyEqualTo(5.5, 0.01, "Approximately 5.5");

        Assert.Equal(5.5, result);
    }

    #endregion
}