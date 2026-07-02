using Benday.Common.Interfaces;

namespace Benday.Common
{
    /// <summary>
    /// Indicates the string identity property for a domain object,
    /// entity framework core entity, CosmosDb entity, or Azure Storage entity.
    /// For EF Core, this typically becomes the primary key.
    /// </summary>
    public interface IStringIdentity : IEntityIdentity<string>
    {
        new string Id { get; set; }
    }
}
