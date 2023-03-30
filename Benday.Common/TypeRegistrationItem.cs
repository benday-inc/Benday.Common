using System;

using Microsoft.Extensions.DependencyInjection;

namespace Benday.Common
{
    /// <summary>
    /// Describes a type mapping that should be registered with
    /// .NET Core dependency injection. 
    /// </summary>
    /// <typeparam name="TService">Service type or interface type</typeparam>
    /// <typeparam name="TImplementation">Implementation type</typeparam>
    public class TypeRegistrationItem<TService, TImplementation> : ITypeRegistrationItem
        where TService : class
        where TImplementation : class, TService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetime">What is the instancing lifetime for this 
        /// type registration?</param>
        public TypeRegistrationItem(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            Lifetime = lifetime;
        }

        /// <summary>
        /// Instancing lifetime for this type mapping
        /// </summary>
        public ServiceLifetime Lifetime { get; set; }

        /// <summary>
        /// Service or interface type
        /// </summary>
        public Type ServiceType => typeof(TService);

        /// <summary>
        /// Implementation type
        /// </summary>
        public Type ImplementationType => typeof(TImplementation);
    }
}
