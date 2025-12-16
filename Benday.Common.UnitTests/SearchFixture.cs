using System;

using Benday.Common.Testing;

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
        SystemUnderTest.Sorts.ShouldNotBeNull("Sorts should not be null.");
    }

    [Fact]
    public void Sorts_ItemCountShouldBeZero_OnInit()
    {
        SystemUnderTest.Sorts.Count.ShouldEqual(0, "Item count was wrong.");
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
        SystemUnderTest.Sorts.Count.ShouldEqual(1, "Item count was wrong.");
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
        SystemUnderTest.Sorts.Count.ShouldEqual(1, "Item count was wrong.");
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
        SystemUnderTest.Sorts.Count.ShouldEqual(2, "Item count was wrong.");
        AssertSort(SystemUnderTest.Sorts[0], expectedSortByValue1, expectedSortDirection);
        AssertSort(SystemUnderTest.Sorts[1], expectedSortByValue2, expectedSortDirection);
    }

    private static void AssertSort(SortBy actual, string expectedSortByValue, string expectedSortDirection)
    {
        actual.ShouldNotBeNull("sortby is null");
        actual.PropertyName.ShouldEqual(expectedSortByValue, "SortByValue");
        actual.Direction.ShouldEqual(expectedSortDirection, "SortDirection");
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
        SystemUnderTest.Sorts.Count.ShouldEqual(1, "Item count was wrong.");
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
        SystemUnderTest.Sorts.Count.ShouldEqual(1, "Item count was wrong.");
        AssertSort(SystemUnderTest.Sorts[0], expectedSortByValue, expectedSortDirection);
    }

    [Fact]
    public void AddSort_ThrowsExceptionOnBogusSortDirection()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = "garbage";

        // act & assert
        Assert.Throws<ArgumentOutOfRangeException>(() => SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection));
    }

    [Fact]
    public void AddSort_ThrowsExceptionOnStringEmptySortDirection()
    {
        // arrange
        var expectedSortByValue = "asdf";
        var expectedSortDirection = string.Empty;

        // act & assert
        Assert.Throws<ArgumentOutOfRangeException>(() => SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection));
    }
}
