using Benday.Common.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.Interfaces.UnitTests;

public class InMemoryOwnedItemRepositoryTests : TestClassBase
{
    public InMemoryOwnedItemRepositoryTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task SaveAndRetrieve()
    {
        // arrange
        var repo = new InMemoryOwnedItemRepository<TestOwnedEntity, string>();
        var entity = new TestOwnedEntity
        {
            Id = Guid.NewGuid().ToString(),
            OwnerId = "owner-1",
            Name = "test item"
        };

        // act
        await repo.SaveAsync(entity);
        var retrieved = await repo.GetByIdAsync("owner-1", entity.Id);

        // assert
        AssertThat.IsNotNull(retrieved, "Entity should exist after save");
        retrieved!.Id.ShouldEqual(entity.Id, "Id should match");
        retrieved.OwnerId.ShouldEqual("owner-1", "OwnerId should match");
        retrieved.Name.ShouldEqual("test item", "Name should match");
    }

    [Fact]
    public async Task GetByOwner_ReturnsOnlyCorrectOwnersEntities()
    {
        // arrange
        var repo = new InMemoryOwnedItemRepository<TestOwnedEntity, string>();

        var entity1 = new TestOwnedEntity { Id = "e1", OwnerId = "owner-1", Name = "item 1" };
        var entity2 = new TestOwnedEntity { Id = "e2", OwnerId = "owner-1", Name = "item 2" };
        var entity3 = new TestOwnedEntity { Id = "e3", OwnerId = "owner-2", Name = "item 3" };

        await repo.SaveAsync(entity1);
        await repo.SaveAsync(entity2);
        await repo.SaveAsync(entity3);

        // act
        var owner1Items = await repo.GetByOwnerAsync("owner-1");
        var owner2Items = await repo.GetByOwnerAsync("owner-2");

        // assert
        owner1Items.Count.ShouldEqual(2, "Owner-1 should have 2 entities");
        owner2Items.Count.ShouldEqual(1, "Owner-2 should have 1 entity");
        owner2Items[0].Name.ShouldEqual("item 3", "Owner-2's entity name should match");
    }

    [Fact]
    public async Task Delete_RemovesEntity()
    {
        // arrange
        var repo = new InMemoryOwnedItemRepository<TestOwnedEntity, string>();
        var entity = new TestOwnedEntity { Id = "e1", OwnerId = "owner-1", Name = "item 1" };

        await repo.SaveAsync(entity);

        // act
        await repo.DeleteAsync(entity);
        var retrieved = await repo.GetByIdAsync("owner-1", "e1");

        // assert
        AssertThat.IsNull(retrieved, "Entity should be null after delete");
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotFound()
    {
        // arrange
        var repo = new InMemoryOwnedItemRepository<TestOwnedEntity, string>();

        // act
        var retrieved = await repo.GetByIdAsync("nonexistent-owner", "nonexistent-id");

        // assert
        AssertThat.IsNull(retrieved, "Should return null for nonexistent entity");
    }

    [Fact]
    public async Task GetByOwner_ReturnsEmptyList_WhenOwnerHasNoEntities()
    {
        // arrange
        var repo = new InMemoryOwnedItemRepository<TestOwnedEntity, string>();

        // act
        var items = await repo.GetByOwnerAsync("nonexistent-owner");

        // assert
        items.Count.ShouldEqual(0, "Should return empty list for nonexistent owner");
    }

    [Fact]
    public async Task Save_OverwritesExistingEntity()
    {
        // arrange
        var repo = new InMemoryOwnedItemRepository<TestOwnedEntity, string>();
        var entity = new TestOwnedEntity { Id = "e1", OwnerId = "owner-1", Name = "original" };
        await repo.SaveAsync(entity);

        // act
        var updated = new TestOwnedEntity { Id = "e1", OwnerId = "owner-1", Name = "updated" };
        await repo.SaveAsync(updated);
        var retrieved = await repo.GetByIdAsync("owner-1", "e1");

        // assert
        AssertThat.IsNotNull(retrieved, "Entity should exist after update");
        retrieved!.Name.ShouldEqual("updated", "Name should reflect the updated value");
    }
}
