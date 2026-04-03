using Benday.Common;
using Benday.Common.Interfaces;

namespace Benday.Common.Interfaces.UnitTests;

public class TestTenantEntity : ITenantItem<string>, IBlobOwner
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TenantId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GetBlobPrefix() => $"{TenantId}/{Id}/";
}

public class TestParentedEntity : IParentedItem<string>, IBlobOwner
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TenantId { get; set; } = string.Empty;
    public string ParentId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GetBlobPrefix() => $"{TenantId}/{ParentId}/{Id}/";
}

public class TestIntEntity : IEntityIdentity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestDeleteableEntity : IEntityIdentity<string>, IDeleteable
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsMarkedForDelete { get; set; }
}

public class TestSimpleEntity : IEntityIdentity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
