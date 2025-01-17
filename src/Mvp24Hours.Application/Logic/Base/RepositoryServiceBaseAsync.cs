//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using FluentValidation;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Validations;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.ValueObjects.Infrastructure;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.Logic
{
    /// <summary>
    /// Asynchronous base service for using repository and unit of work
    /// </summary>
    public abstract class RepositoryServiceBaseAsync<TUoW>
        where TUoW : IUnitOfWorkAsync
    {
        #region [ Properties ]

        private IUnitOfWorkAsync unitOfWork = null;
        private ILoggingService logger = null;

        /// <summary>
        /// Gets unit of work instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual TUoW UnitOfWork => (TUoW)(unitOfWork ??= ServiceProviderHelper.GetService<TUoW>());

        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging => logger ??= ServiceProviderHelper.GetService<ILoggingService>();

        /// <summary>
        /// Maximum amount returned in query
        /// </summary>
        protected virtual int MaxQtyByQueryPage => ConfigurationPropertiesHelper.MaxQtyByQueryPage;

        #endregion

        #region [ Methods ]

        protected virtual Task<bool> Validate<TEntity>(TEntity entity) where TEntity : class, IEntityBase
        {
            try
            {
                var context = ServiceProviderHelper.GetService<INotificationContext>();
                var validator = ServiceProviderHelper.GetService<IValidator<TEntity>>();
                if (validator != null)
                {
                    var validationResult = validator.Validate(entity);
                    if (!validationResult.IsValid)
                    {
                        if (context != null)
                        {
                            var notifications = validationResult.Errors
                                .Select(x => new Notification(x.ErrorCode, x.ErrorMessage, Core.Enums.MessageType.Error))
                                .ToList();

                            context.Add(notifications);
                        }

                        return false.TaskResult();
                    }
                }
                else
                {
                    bool isValidationModel = entity.GetType()?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false;
                    isValidationModel = isValidationModel || (entity.GetType()?.BaseType?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false);

                    if (isValidationModel)
                    {
                        if (!((IValidationModel<TEntity>)entity).IsValid(context))
                        {
                            return false.TaskResult();
                        }
                    }
                }

                return true.TaskResult();
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        #endregion
    }
}
