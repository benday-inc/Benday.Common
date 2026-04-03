using Benday.Common;
using Benday.Common.Interfaces;
using Benday.Common.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.Interfaces.UnitTests;

public class InterfaceHierarchyTests : TestClassBase
{
    public InterfaceHierarchyTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void IInt32Identity_IsAssignableTo_IEntityIdentityOfInt()
    {
        // arrange
        var entity = new TestIntEntity { Id = 42, Name = "test" };

        // act
        var asEntityIdentity = entity as IEntityIdentity<int>;

        // assert
        AssertThat.IsNotNull(asEntityIdentity, "IInt32Identity should be assignable to IEntityIdentity<int>");
        asEntityIdentity!.Id.ShouldEqual(42, "Id should match through IEntityIdentity<int>");
    }

    [Fact]
    public void IStringIdentity_IsAssignableTo_IEntityIdentityOfString()
    {
        // arrange
        var entity = new TestStringEntity { Id = "abc-123", Name = "test" };

        // act
        var asEntityIdentity = entity as IEntityIdentity<string>;

        // assert
        AssertThat.IsNotNull(asEntityIdentity, "IStringIdentity should be assignable to IEntityIdentity<string>");
        asEntityIdentity!.Id.ShouldEqual("abc-123", "Id should match through IEntityIdentity<string>");
    }

    [Fact]
    public void IOwnedItem_HasBothIdAndOwnerId()
    {
        // arrange
        var entity = new TestOwnedEntity();

        // act
        entity.Id = "entity-1";
        entity.OwnerId = "owner-1";

        // assert
        entity.Id.ShouldEqual("entity-1", "Id should be settable and readable");
        entity.OwnerId.ShouldEqual("owner-1", "OwnerId should be settable and readable");
    }

    [Fact]
    public void IOwnedItem_IsAssignableTo_IEntityIdentity()
    {
        // arrange
        var entity = new TestOwnedEntity { Id = "entity-1", OwnerId = "owner-1" };

        // act
        var asEntityIdentity = entity as IEntityIdentity<string>;

        // assert
        AssertThat.IsNotNull(asEntityIdentity, "IOwnedItem<string> should be assignable to IEntityIdentity<string>");
        asEntityIdentity!.Id.ShouldEqual("entity-1", "Id should match through IEntityIdentity<string>");
    }

    [Fact]
    public void IBlobOwner_ReturnsBlobPrefix()
    {
        // arrange
        var entity = new TestOwnedEntity
        {
            Id = "item-42",
            OwnerId = "user-7"
        };

        // act
        var prefix = entity.GetBlobPrefix();

        // assert
        prefix.ShouldEqual("user-7/item-42/", "Blob prefix should be constructed from OwnerId and Id");
    }
}
