namespace Benday.Common
{
    /// <summary>
    /// Indicates the int32 identity property for a domain object or 
    /// entity framework core entity. For EF Core with SQL Server, this typically becomes
    /// the primary key with an incrementing value (@@IDENTITY).
    /// </summary>
    public interface IInt32Identity
    {
        int Id { get; set; }
    }
}
