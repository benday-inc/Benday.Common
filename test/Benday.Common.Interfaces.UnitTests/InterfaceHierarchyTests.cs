using Benday.Common;
using Benday.Common.Interfaces;
using Benday.Common.Testing;
using Xunit;

namespace Benday.Common.Interfaces.UnitTests;

public class InterfaceHierarchyTests : TestClassBase
{
    public InterfaceHierarchyTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void IInt32Identity_IsAssignableTo_IEntityIdentityOfInt()
    {
        // arrange / act
        var isAssignable = typeof(IEntityIdentity<int>).IsAssignableFrom(typeof(IInt32Identity));

        // assert
        AssertThat.IsTrue(isAssignable, "IInt32Identity should be assignable to IEntityIdentity<int>");
    }

    [Fact]
    public void IStringIdentity_IsAssignableTo_IEntityIdentityOfString()
    {
        // arrange / act
        var isAssignable = typeof(IEntityIdentity<string>).IsAssignableFrom(typeof(IStringIdentity));

        // assert
        AssertThat.IsTrue(isAssignable, "IStringIdentity should be assignable to IEntityIdentity<string>");
    }

    [Fact]
    public void ITenantItem_HasBothIdAndTenantId()
    {
        // arrange
        var entity = new TestTenantEntity();

        // act
        entity.Id = "entity-1";
        entity.TenantId = "tenant-1";

        // assert
        entity.Id.ShouldEqual("entity-1", "Id should be settable and readable");
        entity.TenantId.ShouldEqual("tenant-1", "TenantId should be settable and readable");
    }

    [Fact]
    public void IParentedItem_HasIdTenantIdAndParentId()
    {
        // arrange
        var entity = new TestParentedEntity();

        // act
        entity.Id = "entity-1";
        entity.TenantId = "tenant-1";
        entity.ParentId = "parent-1";

        // assert
        entity.Id.ShouldEqual("entity-1", "Id should be settable and readable");
        entity.TenantId.ShouldEqual("tenant-1", "TenantId should be settable and readable");
        entity.ParentId.ShouldEqual("parent-1", "ParentId should be settable and readable");
    }

    [Fact]
    public void IParentedItem_IsAssignableTo_ITenantItem()
    {
        // arrange / act
        var isAssignable = typeof(ITenantItem<string>).IsAssignableFrom(typeof(IParentedItem<string>));

        // assert
        AssertThat.IsTrue(isAssignable, "IParentedItem<string> should be assignable to ITenantItem<string>");
    }

    [Fact]
    public void IBlobOwner_TenantEntity_ReturnsBlobPrefix()
    {
        // arrange
        var entity = new TestTenantEntity
        {
            Id = "item-42",
            TenantId = "tenant-7"
        };

        // act
        var prefix = entity.GetBlobPrefix();

        // assert
        prefix.ShouldEqual("tenant-7/item-42/", "Blob prefix should be constructed from TenantId and Id");
    }

    [Fact]
    public void IBlobOwner_ParentedEntity_ReturnsBlobPrefix()
    {
        // arrange
        var entity = new TestParentedEntity
        {
            Id = "item-42",
            TenantId = "tenant-7",
            ParentId = "parent-3"
        };

        // act
        var prefix = entity.GetBlobPrefix();

        // assert
        prefix.ShouldEqual("tenant-7/parent-3/item-42/", "Blob prefix should include TenantId, ParentId, and Id");
    }

    [Fact]
    public void IDeleteable_SetAndReadIsMarkedForDelete()
    {
        // arrange
        var entity = new TestDeleteableEntity();

        // act
        entity.IsMarkedForDelete = true;

        // assert
        AssertThat.IsTrue(entity.IsMarkedForDelete, "IsMarkedForDelete should be true after setting");
    }
}
