using System;

namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Base identity interface with a generic key type.
    /// Cosmos DB and Table Storage entities use IEntityIdentity{string}.
    /// SQL Server entities use IEntityIdentity{int}.
    /// Follows the same pattern as ASP.NET Core Identity's IdentityUser{TKey}.
    /// </summary>
    public interface IEntityIdentity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
