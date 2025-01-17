//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
namespace Mvp24Hours.Core.Contract.ValueObjects.Logic
{
    /// <summary>
    /// Defines the information needed to access a resource
    /// </summary>
    public interface ILinkResult
    {
        /// <summary>
        /// Resource URI
        /// </summary>
        string Href { get; }
        /// <summary>
        /// Describes how the URI relates to the current resource
        /// </summary>
        string Rel { get; }
        /// <summary>
        /// Method for web request
        /// </summary>
        string Method { get; }
        /// <summary>
        /// Indicates whether the URI is a template or definitive address
        /// </summary>
        bool? IsTemplate { get; }
    }
}
