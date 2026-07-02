using Benday.Common.Testing;
using Xunit;

namespace Benday.Common.Interfaces.UnitTests;

public class InMemoryTenantRepositoryTests : TestClassBase
{
    public InMemoryTenantRepositoryTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task SaveAndRetrieve()
    {
        // arrange
        var repo = new InMemoryTenantRepository<TestTenantEntity, string>();
        var entity = new TestTenantEntity
        {
            Id = Guid.NewGuid().ToString(),
            TenantId = "tenant-1",
            Name = "test item"
        };

        // act
        await repo.SaveAsync(entity);
        var retrieved = await repo.GetByIdAsync("tenant-1", entity.Id);

        // assert
        AssertThat.IsNotNull(retrieved, "Entity should exist after save");
        retrieved!.Id.ShouldEqual(entity.Id, "Id should match");
        retrieved.TenantId.ShouldEqual("tenant-1", "TenantId should match");
        retrieved.Name.ShouldEqual("test item", "Name should match");
    }

    [Fact]
    public async Task GetByTenant_ReturnsOnlyCorrectTenantsEntities()
    {
        // arrange
        var repo = new InMemoryTenantRepository<TestTenantEntity, string>();

        var entity1 = new TestTenantEntity { Id = "e1", TenantId = "tenant-1", Name = "item 1" };
        var entity2 = new TestTenantEntity { Id = "e2", TenantId = "tenant-1", Name = "item 2" };
        var entity3 = new TestTenantEntity { Id = "e3", TenantId = "tenant-2", Name = "item 3" };

        await repo.SaveAsync(entity1);
        await repo.SaveAsync(entity2);
        await repo.SaveAsync(entity3);

        // act
        var tenant1Items = await repo.GetByTenantAsync("tenant-1");
        var tenant2Items = await repo.GetByTenantAsync("tenant-2");

        // assert
        tenant1Items.Count.ShouldEqual(2, "Tenant-1 should have 2 entities");
        tenant2Items.Count.ShouldEqual(1, "Tenant-2 should have 1 entity");
        tenant2Items[0].Name.ShouldEqual("item 3", "Tenant-2's entity name should match");
    }

    [Fact]
    public async Task Delete_RemovesEntity()
    {
        // arrange
        var repo = new InMemoryTenantRepository<TestTenantEntity, string>();
        var entity = new TestTenantEntity { Id = "e1", TenantId = "tenant-1", Name = "item 1" };

        await repo.SaveAsync(entity);

        // act
        await repo.DeleteAsync(entity);
        var retrieved = await repo.GetByIdAsync("tenant-1", "e1");

        // assert
        AssertThat.IsNull(retrieved, "Entity should be null after delete");
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotFound()
    {
        // arrange
        var repo = new InMemoryTenantRepository<TestTenantEntity, string>();

        // act
        var retrieved = await repo.GetByIdAsync("nonexistent-tenant", "nonexistent-id");

        // assert
        AssertThat.IsNull(retrieved, "Should return null for nonexistent entity");
    }

    [Fact]
    public async Task GetByTenant_ReturnsEmptyList_WhenTenantHasNoEntities()
    {
        // arrange
        var repo = new InMemoryTenantRepository<TestTenantEntity, string>();

        // act
        var items = await repo.GetByTenantAsync("nonexistent-tenant");

        // assert
        items.Count.ShouldEqual(0, "Should return empty list for nonexistent tenant");
    }

    [Fact]
    public async Task Save_OverwritesExistingEntity()
    {
        // arrange
        var repo = new InMemoryTenantRepository<TestTenantEntity, string>();
        var entity = new TestTenantEntity { Id = "e1", TenantId = "tenant-1", Name = "original" };
        await repo.SaveAsync(entity);

        // act
        var updated = new TestTenantEntity { Id = "e1", TenantId = "tenant-1", Name = "updated" };
        await repo.SaveAsync(updated);
        var retrieved = await repo.GetByIdAsync("tenant-1", "e1");

        // assert
        AssertThat.IsNotNull(retrieved, "Entity should exist after update");
        retrieved!.Name.ShouldEqual("updated", "Name should reflect the updated value");
    }
}
