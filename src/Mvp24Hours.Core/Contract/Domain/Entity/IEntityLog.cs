﻿//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
namespace Mvp24Hours.Core.Contract.Domain.Entity
{
    /// <summary>
    /// Represents an entity with data to log
    ///  <see cref="Mvp24Hours.Core.Contract.Domain.Entity.IEntityBase"/>
    /// </summary>
    public interface IEntityLog<T> : IEntityDateLog
    {
        /// <summary>
        /// Registration of who requested the creation of this entity
        /// </summary>
        T CreatedBy { get; set; }
        /// <summary>
        /// Registration of who requested the modification of this entity
        /// </summary>
        T ModifiedBy { get; set; }
        /// <summary>
        /// Record of who requested the logical exclusion of that entity
        /// </summary>
        T RemovedBy { get; set; }
    }
}