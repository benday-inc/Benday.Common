using Benday.Common;
using Benday.Common.Interfaces;

namespace Benday.Common.Interfaces.UnitTests;

public class TestOwnedEntity : IOwnedItem<string>, IBlobOwner
{
    public string Id { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GetBlobPrefix() => $"{OwnerId}/{Id}/";
}

public class TestIntEntity : IInt32Identity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestStringEntity : IStringIdentity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class TestSimpleEntity : IEntityIdentity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
