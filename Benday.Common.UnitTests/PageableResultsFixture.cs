using System.Collections.Generic;
using System.Linq;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests;

public class PageableResultsFixture : TestClassBase
{
    public PageableResultsFixture(ITestOutputHelper output) : base(output)
    {
        _systemUnderTest = null;
    }

    private PageableResults<string>? _systemUnderTest;
    public PageableResults<string> SystemUnderTest
    {
        get
        {
            _systemUnderTest ??= new PageableResults<string>();
            return _systemUnderTest;
        }
    }

    [Fact]
    public void Results_WhenUninitialized_IsNotNull()
    {
        // act
        var results = SystemUnderTest.Results;

        // assert
        results.ShouldNotBeNull("Results should not be null.");
    }

    [Fact]
    public void TotalCount_WhenUninitialized_IsZero()
    {
        // act
        var totalCount = SystemUnderTest.TotalCount;

        // assert
        totalCount.ShouldEqual(0, "TotalCount should be zero.");
    }

    [Fact]
    public void ItemsPerPage_DefaultValue()
    {
        // arrange
        var expected = 10;

        // act
        var actual = SystemUnderTest.ItemsPerPage;

        // assert
        actual.ShouldEqual(expected, "ItemsPerPage was wrong.");
    }

    [Fact]
    public void TotalCountReturnsTotalOfAllRecords()
    {
        // arrange
        var expectedNumberOfRecords = 300;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

        // act
        var actual = SystemUnderTest.TotalCount;

        // assert
        actual.ShouldEqual(expectedNumberOfRecords, "TotalCount was wrong.");
    }

    [Fact]
    public void PageCount_NoRemainder()
    {
        // arrange
        var expectedNumberOfRecords = 300;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedPageCount = 30;

        // act
        var actual = SystemUnderTest.PageCount;

        // assert
        actual.ShouldEqual(expectedPageCount, "PageCount was wrong.");
    }

    [Fact]
    public void PageCount_Remainder()
    {
        // arrange
        var expectedNumberOfRecords = 305;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedPageCount = 31;

        // act
        var actual = SystemUnderTest.PageCount;

        // assert
        actual.ShouldEqual(expectedPageCount, "PageCount was wrong.");
    }

    [Fact]
    public void PageCount_LessThanOnePage()
    {
        // arrange
        var expectedNumberOfRecords = 5;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedPageCount = 1;

        // act
        var actual = SystemUnderTest.PageCount;

        // assert
        actual.ShouldEqual(expectedPageCount, "PageCount was wrong.");
    }

    [Fact]
    public void PageCount_NoRecords()
    {
        // arrange
        var expectedNumberOfRecords = 0;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedPageCount = 0;

        // act
        var actual = SystemUnderTest.PageCount;

        // assert
        actual.ShouldEqual(expectedPageCount, "PageCount was wrong.");
    }

    [Fact]
    public void CurrentPage_SetToOneOnInitialize()
    {
        // arrange
        var expectedNumberOfRecords = 305;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedCurrentPage = 1;

        // act
        var actual = SystemUnderTest.CurrentPage;

        // assert
        actual.ShouldEqual(expectedCurrentPage, "CurrentPage was wrong.");
    }

    [Fact]
    public void CurrentPage_SetToHigherThanPageCountSetsToHighestPage()
    {
        // arrange
        var expectedNumberOfRecords = 100;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedCurrentPage = 10;

        // act
        SystemUnderTest.CurrentPage = 300;

        // assert
        SystemUnderTest.CurrentPage.ShouldEqual(expectedCurrentPage, "CurrentPage was wrong.");
    }

    [Fact]
    public void CurrentPage_SetToLowerThanPageCountSetsToOne()
    {
        // arrange
        var expectedNumberOfRecords = 100;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedCurrentPage = 1;

        // act
        SystemUnderTest.CurrentPage = -300;

        // assert
        SystemUnderTest.CurrentPage.ShouldEqual(expectedCurrentPage, "CurrentPage was wrong.");
    }

    [Fact]
    public void CurrentPage_SetToValidPageNumber()
    {
        // arrange
        var expectedNumberOfRecords = 100;
        SystemUnderTest.ItemsPerPage = 10;
        SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));
        var expectedCurrentPage = 5;

        // act
        SystemUnderTest.CurrentPage = expectedCurrentPage;

        // assert
        SystemUnderTest.CurrentPage.ShouldEqual(expectedCurrentPage, "CurrentPage was wrong.");
    }

    [Fact]
    public void PageValues_InitializesToPage1Values()
    {
        // arrange
        var expectedNumberOfRecords = 100;
        var expectedItemsPerPage = 10;
        SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
        var allValues = CreateModels(expectedNumberOfRecords);
        SystemUnderTest.Initialize(allValues);
        var expectedPage1Values = allValues.Take(expectedItemsPerPage).ToList();

        // act
        var actualPageValues = SystemUnderTest.PageValues;

        // assert
        actualPageValues.AsEnumerable().ShouldEqual(expectedPage1Values, "Page values were wrong.");
        actualPageValues.Count.ShouldEqual(expectedItemsPerPage, "Number of values on page was wrong.");
    }

    [Fact]
    public void PageValues_ChangePageUpdatesPageValues_Page2()
    {
        // arrange
        var expectedNumberOfRecords = 100;
        var expectedItemsPerPage = 10;
        SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
        var allValues = CreateModels(expectedNumberOfRecords);
        SystemUnderTest.Initialize(allValues);
        var expectedPage2Values = allValues.Skip(expectedItemsPerPage).Take(expectedItemsPerPage).ToList();

        // act
        SystemUnderTest.CurrentPage = 2;
        var actualPageValues = SystemUnderTest.PageValues;

        // assert
        actualPageValues.AsEnumerable().ShouldEqual(expectedPage2Values, "Page values were wrong.");
        actualPageValues.Count.ShouldEqual(expectedItemsPerPage, "Number of values on page was wrong.");
    }

    [Fact]
    public void PageValues_ChangePageUpdatesPageValues_Page3()
    {
        // arrange
        var expectedNumberOfRecords = 100;
        var expectedItemsPerPage = 10;
        SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
        var allValues = CreateModels(expectedNumberOfRecords);
        SystemUnderTest.Initialize(allValues);
        var expectedPage3Values = allValues.Skip(expectedItemsPerPage * 2).Take(expectedItemsPerPage).ToList();

        // act
        SystemUnderTest.CurrentPage = 3;
        var actualPageValues = SystemUnderTest.PageValues;

        // assert
        actualPageValues.AsEnumerable().ShouldEqual(expectedPage3Values, "Page values were wrong.");
        actualPageValues.Count.ShouldEqual(expectedItemsPerPage, "Number of values on page was wrong.");
    }

    [Fact]
    public void PageValues_ChangePageUpdatesPageValues_LastPage_Remainder()
    {
        // arrange
        var expectedNumberOfRecords = 25;
        var expectedItemsPerPage = 10;
        SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
        var allValues = CreateModels(expectedNumberOfRecords);
        SystemUnderTest.Initialize(allValues);
        var expectedPage3Values = allValues.Skip(expectedItemsPerPage * 2).Take(expectedItemsPerPage).ToList();

        // act
        SystemUnderTest.CurrentPage = 3;
        var actualPageValues = SystemUnderTest.PageValues;

        // assert
        actualPageValues.AsEnumerable().ShouldEqual(expectedPage3Values, "Page values were wrong.");
        actualPageValues.Count.ShouldEqual(5, "Number of values on page was wrong.");
    }

    private static List<string> CreateModels(int expectedNumberOfRecords)
    {
        var returnValues = new List<string>();
        for (var i = 0; i < expectedNumberOfRecords; i++)
        {
            returnValues.Add($"item {i}");
        }
        return returnValues;
    }
}
