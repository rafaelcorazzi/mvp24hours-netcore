//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Data
{
    /// <summary>
    /// Design Pattern: Repository
    /// Description: Mediation between domain and data mapping layers using a collection as 
    /// an interface for accessing domain objects. (Martin Fowler)
    /// Learn more: http://martinfowler.com/eaaCatalog/repository.html
    /// </summary>
    /// <typeparam name="T">Represents an model</typeparam>
    public interface IRepositoryCacheAsync<T>
    {
        /// <summary>
        /// Get model by key
        /// </summary>
        Task<T> GetAsync(string key);
        /// <summary>
        /// Get string by key
        /// </summary>
        Task<string> GetStringAsync(string key);
        /// <summary>
        /// Register model by key
        /// </summary>
        Task SetAsync(string key, T model);
        /// <summary>
        /// Register string by key
        /// </summary>
        Task SetStringAsync(string key, string value);
        /// <summary>
        /// Remove model/string by key
        /// </summary>
        Task RemoveAsync(string key);
    }
}
