using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Common.Testing.UnitTests.Assertions;

/// <summary>
/// Tests that confirm <see cref="AssertThatCollection"/> compares collection elements
/// by value (via <see cref="EqualityComparer{T}.Default"/>) rather than by reference.
/// </summary>
public class AssertThatCollectionFixture : TestClassBase
{
    public AssertThatCollectionFixture(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    /// Builds a non-interned copy of a string so that reference equality would fail
    /// but value equality succeeds. This lets the tests prove value comparison rather
    /// than accidentally passing because of string interning.
    /// </summary>
    private static string NotInterned(string value) => new string(value.ToCharArray());

    #region AreEqual - string

    [Fact]
    public void AreEqual_ListOfString_And_StringArray_WithEqualValues_DoesNotThrow()
    {
        // Arrange
        var expected = new List<string> { "alpha", "beta", "gamma" };
        string[] actual = { "alpha", "beta", "gamma" };

        // Act & Assert - different collection types, same values, should be equal by value
        AssertThatCollection.AreEqual(expected, actual, "String collections with equal values should match");
    }

    [Fact]
    public void AreEqual_StringCollections_WithDistinctReferencesButEqualValues_DoesNotThrow()
    {
        // Arrange - actual holds non-interned strings, so element references differ from expected
        var expected = new List<string> { "alpha", "beta", "gamma" };
        var actual = new List<string>
        {
            NotInterned("alpha"),
            NotInterned("beta"),
            NotInterned("gamma")
        };

        // Sanity check: the element references really are different instances
        Assert.False(ReferenceEquals(expected[0], actual[0]),
            "Test setup error: strings should be distinct references to prove value comparison");

        // Act & Assert - comparison must be by value, not by reference
        AssertThatCollection.AreEqual(expected, actual, "Strings should compare by value, not reference");
    }

    [Fact]
    public void AreEqual_StringCollections_WithDifferentValues_Throws()
    {
        // Arrange
        var expected = new List<string> { "alpha", "beta", "gamma" };
        string[] actual = { "alpha", "beta", "delta" };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual, "Different string values should not match"));

        Assert.Contains("Different string values should not match", ex.Message);
    }

    #endregion

    #region AreEqual - int

    [Fact]
    public void AreEqual_ListOfInt_And_IntArray_WithEqualValues_DoesNotThrow()
    {
        // Arrange
        var expected = new List<int> { 1, 2, 3 };
        int[] actual = { 1, 2, 3 };

        // Act & Assert
        AssertThatCollection.AreEqual(expected, actual, "Int collections with equal values should match");
    }

    [Fact]
    public void AreEqual_IntCollections_WithDifferentValues_Throws()
    {
        // Arrange
        var expected = new List<int> { 1, 2, 3 };
        int[] actual = { 1, 2, 4 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual, "Different int values should not match"));

        Assert.Contains("Different int values should not match", ex.Message);
    }

    [Fact]
    public void AreEqual_IntCollections_WithDifferentCounts_Throws()
    {
        // Arrange
        var expected = new List<int> { 1, 2, 3 };
        int[] actual = { 1, 2 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual, "Different counts should not match"));

        Assert.Contains("Different counts should not match", ex.Message);
    }

    #endregion

    #region AreEqual - DateTime

    [Fact]
    public void AreEqual_ListOfDateTime_And_DateTimeArray_WithEqualValues_DoesNotThrow()
    {
        // Arrange - build the "actual" values from a computation so they are distinct
        // DateTime instances that nonetheless carry the same value.
        var expected = new List<DateTime>
        {
            new DateTime(2024, 1, 15),
            new DateTime(2024, 6, 30),
            new DateTime(2025, 12, 1)
        };
        DateTime[] actual =
        {
            new DateTime(2024, 1, 14).AddDays(1),
            new DateTime(2024, 6, 29).AddDays(1),
            new DateTime(2025, 11, 30).AddDays(1)
        };

        // Act & Assert
        AssertThatCollection.AreEqual(expected, actual, "DateTime collections with equal values should match");
    }

    [Fact]
    public void AreEqual_DateTimeCollections_WithDifferentValues_Throws()
    {
        // Arrange
        var expected = new List<DateTime> { new DateTime(2024, 1, 15) };
        DateTime[] actual = { new DateTime(2024, 1, 16) };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual, "Different DateTime values should not match"));

        Assert.Contains("Different DateTime values should not match", ex.Message);
    }

    #endregion

    #region Contains - value comparison

    [Fact]
    public void Contains_StringCollection_WithNonInternedMatch_DoesNotThrow()
    {
        // Arrange
        var collection = new List<string> { "alpha", "beta", "gamma" };
        var item = NotInterned("beta");

        Assert.False(ReferenceEquals(collection[1], item),
            "Test setup error: item should be a distinct reference to prove value comparison");

        // Act & Assert - Contains must match by value
        AssertThatCollection.Contains(collection, item, "Should contain 'beta' by value");
    }

    [Fact]
    public void Contains_DateTimeCollection_WithEqualValue_DoesNotThrow()
    {
        // Arrange
        var collection = new List<DateTime>
        {
            new DateTime(2024, 1, 15),
            new DateTime(2024, 6, 30)
        };
        var item = new DateTime(2024, 6, 29).AddDays(1);

        // Act & Assert
        AssertThatCollection.Contains(collection, item, "Should contain the DateTime by value");
    }

    #endregion

    #region Sample types and comparers for comparer-based tests

    private sealed class Person
    {
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }

        public override string ToString() => $"Person({Id}, {Name})";
    }

    /// <summary>
    /// Compares <see cref="Person"/> by <see cref="Person.Id"/> only. Person does not override
    /// Equals, so without this comparer the default comparer uses reference equality.
    /// </summary>
    private sealed class PersonByIdComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person? x, Person? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(Person obj) => obj.Id.GetHashCode();
    }

    #endregion

    #region Default comparer falls back to reference equality (documents the gap)

    [Fact]
    public void AreEqual_CustomType_WithoutComparer_UsesReferenceEquality_Throws()
    {
        // Arrange - distinct instances with equal Id; Person has no Equals override
        var expected = new List<Person> { new Person(1, "Ann") };
        var actual = new List<Person> { new Person(1, "Ann") };

        // Act & Assert - the default comparer compares by reference, so this fails
        Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual, "Reference equality does not match distinct instances"));
    }

    #endregion

    #region AreEqual - IEqualityComparer<T> overload

    [Fact]
    public void AreEqual_WithComparer_CustomType_EqualByKey_DoesNotThrow()
    {
        // Arrange
        var expected = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };
        var actual = new List<Person> { new Person(1, "ignored"), new Person(2, "ignored") };

        // Act & Assert - comparer only looks at Id
        AssertThatCollection.AreEqual(expected, actual, new PersonByIdComparer(), "People should match by id");
    }

    [Fact]
    public void AreEqual_WithComparer_CustomType_DifferentKey_Throws()
    {
        // Arrange
        var expected = new List<Person> { new Person(1, "Ann") };
        var actual = new List<Person> { new Person(99, "Ann") };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual, new PersonByIdComparer(), "Ids differ"));

        Assert.Contains("Ids differ", ex.Message);
    }

    [Fact]
    public void AreEqual_WithNullComparer_Throws()
    {
        var expected = new List<int> { 1 };
        var actual = new List<int> { 1 };

        Assert.Throws<ArgumentNullException>(() =>
            AssertThatCollection.AreEqual(expected, actual, (IEqualityComparer<int>)null!, "message"));
    }

    #endregion

    #region AreEqual - Action<T,T> pairwise overload

    [Fact]
    public void AreEqual_WithElementAssertion_AllPairsMatch_DoesNotThrow()
    {
        // Arrange
        var expected = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };
        var actual = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };

        // Act & Assert - custom per-element comparison
        AssertThatCollection.AreEqual(expected, actual,
            (exp, act) =>
            {
                exp.Id.ShouldEqual(act.Id, "ids should match");
                exp.Name.ShouldEqual(act.Name, "names should match");
            },
            "People should match member-by-member");
    }

    [Fact]
    public void AreEqual_WithElementAssertion_PairFails_ThrowsWithIndex()
    {
        // Arrange
        var expected = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };
        var actual = new List<Person> { new Person(1, "Ann"), new Person(2, "Robert") };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual,
                (exp, act) => exp.Name.ShouldEqual(act.Name, "names should match"),
                "People should match"));

        Assert.Contains("index 1", ex.Message);
    }

    [Fact]
    public void AreEqual_WithElementAssertion_DifferentCounts_Throws()
    {
        var expected = new List<int> { 1, 2, 3 };
        var actual = new List<int> { 1, 2 };

        Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEqual(expected, actual,
                (exp, act) => exp.ShouldEqual(act, "should match"),
                "counts differ"));
    }

    #endregion

    #region AreEquivalent - unordered content equality

    [Fact]
    public void AreEquivalent_SameContentsDifferentOrder_DoesNotThrow()
    {
        // Arrange
        var expected = new List<int> { 1, 2, 3 };
        int[] actual = { 3, 1, 2 };

        // Act & Assert
        AssertThatCollection.AreEquivalent(expected, actual, "Same contents, order ignored");
    }

    [Fact]
    public void AreEquivalent_StringContentsDifferentOrder_DoesNotThrow()
    {
        var expected = new List<string> { "alpha", "beta", "gamma" };
        string[] actual = { NotInterned("gamma"), NotInterned("alpha"), NotInterned("beta") };

        AssertThatCollection.AreEquivalent(expected, actual, "Same string contents, order ignored");
    }

    [Fact]
    public void AreEquivalent_RespectsDuplicateMultiplicity_Throws()
    {
        // Arrange - same distinct values but different number of occurrences
        var expected = new List<int> { 1, 1, 2 };
        int[] actual = { 1, 2, 2 };

        // Act & Assert
        Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEquivalent(expected, actual, "Multiplicity must match"));
    }

    [Fact]
    public void AreEquivalent_DifferentContents_Throws()
    {
        var expected = new List<int> { 1, 2, 3 };
        int[] actual = { 1, 2, 4 };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AreEquivalent(expected, actual, "Contents differ"));

        Assert.Contains("Contents differ", ex.Message);
    }

    [Fact]
    public void AreEquivalent_WithComparer_MatchesByKeyIgnoringOrder_DoesNotThrow()
    {
        var expected = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };
        var actual = new List<Person> { new Person(2, "x"), new Person(1, "y") };

        AssertThatCollection.AreEquivalent(expected, actual, new PersonByIdComparer(), "Match by id, order ignored");
    }

    #endregion

    #region Contains / DoesNotContain - comparer overloads

    [Fact]
    public void Contains_WithComparer_MatchesByKey_DoesNotThrow()
    {
        var collection = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };

        AssertThatCollection.Contains(collection, new Person(2, "different name"), new PersonByIdComparer(),
            "Should contain a person with id 2");
    }

    [Fact]
    public void DoesNotContain_WithComparer_NoKeyMatch_DoesNotThrow()
    {
        var collection = new List<Person> { new Person(1, "Ann") };

        AssertThatCollection.DoesNotContain(collection, new Person(99, "Ann"), new PersonByIdComparer(),
            "Should not contain a person with id 99");
    }

    #endregion

    #region Subset / Superset / Unique - comparer overloads

    [Fact]
    public void IsSubsetOf_WithComparer_MatchesByKey_DoesNotThrow()
    {
        var subset = new List<Person> { new Person(2, "x") };
        var superset = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };

        AssertThatCollection.IsSubsetOf(subset, superset, new PersonByIdComparer(), "Should be subset by id");
    }

    [Fact]
    public void IsSupersetOf_WithComparer_MatchesByKey_DoesNotThrow()
    {
        var superset = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };
        var subset = new List<Person> { new Person(1, "x") };

        AssertThatCollection.IsSupersetOf(superset, subset, new PersonByIdComparer(), "Should be superset by id");
    }

    [Fact]
    public void HasUniqueElements_WithComparer_DuplicateByKey_Throws()
    {
        var collection = new List<Person> { new Person(1, "Ann"), new Person(1, "Bob") };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.HasUniqueElements(collection, new PersonByIdComparer(), "Ids should be unique"));

        Assert.Contains("Ids should be unique", ex.Message);
    }

    #endregion

    #region AllSatisfy - element inspector

    [Fact]
    public void AllSatisfy_WhenAllElementsPass_DoesNotThrow()
    {
        var people = new List<Person> { new Person(1, "Ann"), new Person(2, "Bob") };

        AssertThatCollection.AllSatisfy(people,
            p => p.Name.ShouldNotBeNullOrEmpty("every person has a name"),
            "all people are valid");
    }

    [Fact]
    public void AllSatisfy_WhenAnElementFails_ThrowsWithIndex()
    {
        var people = new List<Person> { new Person(1, "Ann"), new Person(2, "") };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThatCollection.AllSatisfy(people,
                p => p.Name.ShouldNotBeNullOrEmpty("every person has a name"),
                "all people are valid"));

        Assert.Contains("index 1", ex.Message);
        Assert.Contains("all people are valid", ex.Message);
    }

    #endregion
}
