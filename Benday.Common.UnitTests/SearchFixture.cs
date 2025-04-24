using System;

using Benday.Common.Testing;

using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests;

public class SearchFixture : TestClassBase
{
    public SearchFixture(ITestOutputHelper output) : base(output)
    {
        _systemUnderTest = null;
    }

    private Search? _systemUnderTest;
    public Search SystemUnderTest
    {
        get
        {
            _systemUnderTest ??= new Search();
            return _systemUnderTest;
        }
    }

    [Fact]
    public void Sorts_NotNull_OnInit()
    {
        SystemUnderTest.Sorts.Should().NotBeNull();
    }

    [Fact]
    public void Sorts_ItemCountShouldBeZero_OnInit()
    {
        SystemUnderTest.Sorts.Count.Should().Be(0, "Item count was wrong.");
    }

    [Fact]
    public void AddSort_DefaultSortDirectionIsAscending()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = SearchConstants.SortDirectionAscending;

        // act
        SystemUnderTest.AddSort(expectedSortByValue);

        // assert
        SystemUnderTest.Sorts.Count.Should().Be(1, "Item count was wrong.");
        var actual = SystemUnderTest.Sorts[0];
        AssertSort(actual, expectedSortByValue, expectedSortDirection);
    }

    [Fact]
    public void AddSort_IgnoresDuplicateSortValue()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = SearchConstants.SortDirectionAscending;

        // act
        SystemUnderTest.AddSort(expectedSortByValue);
        SystemUnderTest.AddSort(expectedSortByValue);

        // assert
        SystemUnderTest.Sorts.Count.Should().Be(1, "Item count was wrong.");
        var actual = SystemUnderTest.Sorts[0];
        AssertSort(actual, expectedSortByValue, expectedSortDirection);
    }

    [Fact]
    public void AddSort_AddTwoSorts()
    {
        // arrange
        var expectedSortByValue1 = "asdf";
        var expectedSortByValue2 = "qwer";
        var expectedSortDirection = SearchConstants.SortDirectionAscending;

        // act
        SystemUnderTest.AddSort(expectedSortByValue1);
        SystemUnderTest.AddSort(expectedSortByValue2);

        // assert
        SystemUnderTest.Sorts.Count.Should().Be(2, "Item count was wrong.");
        AssertSort(SystemUnderTest.Sorts[0], expectedSortByValue1, expectedSortDirection);
        AssertSort(SystemUnderTest.Sorts[1], expectedSortByValue2, expectedSortDirection);
    }

    private static void AssertSort(SortBy actual, string expectedSortByValue, string expectedSortDirection)
    {
        actual.Should().NotBeNull("sortby is null");
        actual.PropertyName.Should().Be(expectedSortByValue, "SortByValue");
        actual.Direction.Should().Be(expectedSortDirection, "SortDirection");
    }

    [Fact]
    public void AddSort_Ascending()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = SearchConstants.SortDirectionAscending;

        // act
        SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

        // assert
        SystemUnderTest.Sorts.Count.Should().Be(1, "Item count was wrong.");
        AssertSort(SystemUnderTest.Sorts[0], expectedSortByValue, expectedSortDirection);
    }

    [Fact]
    public void AddSort_Descending()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = SearchConstants.SortDirectionDescending;

        // act
        SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

        // assert
        SystemUnderTest.Sorts.Count.Should().Be(1, "Item count was wrong.");
        AssertSort(SystemUnderTest.Sorts[0], expectedSortByValue, expectedSortDirection);
    }

    [Fact]
    public void AddSort_ThrowsExceptionOnBogusSortDirection()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = "garbage";

        // act
        Action act = () => SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

        // assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AddSort_ThrowsExceptionOnStringEmptySortDirection()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = string.Empty;

        // act
        Action act = () => SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

        // assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
