using System;

using Microsoft.Extensions.DependencyInjection;

namespace Benday.Common
{
    /// <summary>
    /// Interface for describing a type that should be registered with
    /// .NET Core dependency injection. It describes the service lifetime,
    /// interface data type, and concrete implementation.
    /// </summary>
    public interface ITypeRegistrationItem
    {
        /// <summary>
        /// Type that implements the interface described by the service type property.
        /// </summary>
        Type ImplementationType { get; }
        
        /// <summary>
        /// What is the lifetime registration type in .NET Core dependency injection.
        /// </summary>
        ServiceLifetime Lifetime { get; set; }
        
        /// <summary>
        /// Interface type for this registration
        /// </summary>
        Type ServiceType { get; }
    }
}
