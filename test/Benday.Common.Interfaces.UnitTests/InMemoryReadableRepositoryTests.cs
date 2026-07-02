using Benday.Common.Testing;
using Xunit;

namespace Benday.Common.Interfaces.UnitTests;

public class InMemoryReadableRepositoryTests : TestClassBase
{
    public InMemoryReadableRepositoryTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task SaveAndRetrieveById()
    {
        // arrange
        var repo = new InMemoryReadableRepository<TestSimpleEntity, int>();
        var entity = new TestSimpleEntity { Id = 1, Name = "test item" };

        // act
        await repo.SaveAsync(entity);
        var retrieved = await repo.GetByIdAsync(1);

        // assert
        AssertThat.IsNotNull(retrieved, "Entity should exist after save");
        retrieved!.Id.ShouldEqual(1, "Id should match");
        retrieved.Name.ShouldEqual("test item", "Name should match");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSavedEntities()
    {
        // arrange
        var repo = new InMemoryReadableRepository<TestSimpleEntity, int>();
        await repo.SaveAsync(new TestSimpleEntity { Id = 1, Name = "item 1" });
        await repo.SaveAsync(new TestSimpleEntity { Id = 2, Name = "item 2" });
        await repo.SaveAsync(new TestSimpleEntity { Id = 3, Name = "item 3" });

        // act
        var all = await repo.GetAllAsync();

        // assert
        all.Count.ShouldEqual(3, "Should return all 3 saved entities");
    }

    [Fact]
    public async Task Delete_RemovesEntity()
    {
        // arrange
        var repo = new InMemoryReadableRepository<TestSimpleEntity, int>();
        var entity = new TestSimpleEntity { Id = 1, Name = "item 1" };
        await repo.SaveAsync(entity);

        // act
        await repo.DeleteAsync(entity);
        var retrieved = await repo.GetByIdAsync(1);

        // assert
        AssertThat.IsNull(retrieved, "Entity should be null after delete");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // arrange
        var repo = new InMemoryReadableRepository<TestSimpleEntity, int>();

        // act
        var retrieved = await repo.GetByIdAsync(999);

        // assert
        AssertThat.IsNull(retrieved, "Should return null for nonexistent entity");
    }

    [Fact]
    public async Task Save_OverwritesExistingEntity()
    {
        // arrange
        var repo = new InMemoryReadableRepository<TestSimpleEntity, int>();
        await repo.SaveAsync(new TestSimpleEntity { Id = 1, Name = "original" });

        // act
        await repo.SaveAsync(new TestSimpleEntity { Id = 1, Name = "updated" });
        var retrieved = await repo.GetByIdAsync(1);

        // assert
        AssertThat.IsNotNull(retrieved, "Entity should exist after update");
        retrieved!.Name.ShouldEqual("updated", "Name should reflect the updated value");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenEmpty()
    {
        // arrange
        var repo = new InMemoryReadableRepository<TestSimpleEntity, int>();

        // act
        var all = await repo.GetAllAsync();

        // assert
        all.Count.ShouldEqual(0, "Should return empty list when no entities saved");
    }
}
