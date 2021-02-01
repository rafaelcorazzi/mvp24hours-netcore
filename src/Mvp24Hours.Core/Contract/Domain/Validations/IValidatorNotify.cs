﻿//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Specifications;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;

namespace Mvp24Hours.Core.Contract.Domain.Validations
{
    /// <summary>
    /// Specification testing notification manager
    /// </summary>
    /// <typeparam name="T">Test model</typeparam>
    /// <example>
    /// <code>
    ///     var validator = new ValidatorEntityNotify{Product}()
    ///         .AddSpecification{SpecialCategoryAllowsOneProductSpecification}();
    ///         .AddSpecification{IsNotSpecialCategorySpecification}()
    ///         .AddSpecification{CategoryHasNotProductSpecification}()
    ///     if (!validator.Validate(model))
    ///         return 0;
    /// </code>
    /// </example>
    public interface IValidatorNotify<T>
    {
        /// <summary>
        /// Notification context
        /// </summary>
        INotificationContext Context { get; }
        /// <summary>
        /// Indicates whether the notification context is valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <typeparam name="U">Test model</typeparam>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification<U>()
            where U : ISpecification<T>, new();
        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <typeparam name="U">Test model</typeparam>
        /// <param name="keyValidation">Reference key for specification not satisfied</param>
        /// <param name="messageValidation">Message for specification not satisfied</param>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification<U>(string keyValidation, string messageValidation)
            where U : ISpecification<T>, new();
        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <typeparam name="U">Test model</typeparam>
        /// <param name="specification">Specification for testing model</param>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification<U>(ISpecification<T> specification)
            where U : ISpecification<T>;
        /// <summary>
        /// Adds specification for testing
        /// </summary>
        /// <typeparam name="U">Test model</typeparam>
        /// <param name="specificationValidator">Validation specification</param>
        /// <returns>IValidatorNotify{T}</returns>
        IValidatorNotify<T> AddSpecification<U>(ISpecificationValidator<T> specificationValidator)
            where U : ISpecificationValidator<T>;

        /// <summary>
        /// Tests whether the model meets all added specifications
        /// </summary>
        /// <param name="Candidate">Model object for testing</param>
        /// <returns>true|false</returns>
        bool Validate(T Candidate);
    }
}