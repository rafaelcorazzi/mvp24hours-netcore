//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Extensions.Data;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;

namespace Mvp24Hours.Infrastructure.Data.EFCore
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IRepository"/>
    /// </summary>
    public abstract class RepositoryBase<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        protected RepositoryBase(DbContext _dbContext)
        {
            this.dbContext = _dbContext ?? throw new ArgumentNullException("dbContext");
            this.dbEntities = _dbContext.Set<T>();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Database context
        /// </summary>
        protected readonly DbContext dbContext;
        /// <summary>
        /// Represents relationship with entities in the database
        /// </summary>
        protected DbSet<T> dbEntities;
        /// <summary>
        /// Gets the value of the user logged in the context or logged into the database
        /// </summary>
        protected abstract object EntityLogBy { get; }

        private static bool? _enableReadUncommitedQuery;
        protected static bool EnableReadUncommitedQuery
        {
            get
            {
                if (_enableReadUncommitedQuery == null)
                {
                    string value = ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:ReadUncommitedQuery");
                    _enableReadUncommitedQuery = value.ToBoolean(false);
                }
                return (bool)_enableReadUncommitedQuery;
            }
        }

        #endregion

        #region [ Methods ]
        /// <summary>
        /// Gets database query with clause and aggregation of relationships
        /// </summary>
        protected IQueryable<T> GetQuery(IPagingCriteria criteria, bool onlyNavigation = false)
        {
            // cria query
            var query = this.dbEntities.AsQueryable();
            return GetQuery(query, criteria, onlyNavigation);
        }
        /// <summary>
        /// Gets database query with clause and aggregation of relationships
        /// </summary>
        protected IQueryable<T> GetQuery(IQueryable<T> query, IPagingCriteria criteria, bool onlyNavigation = false)
        {
            var ordered = false;

            if (!onlyNavigation)
            {
                int offset = 0;
                int limit = MaxQtyByQueryPage;

                if (criteria != null)
                {
                    // ordination
                    if (criteria is IPagingCriteriaExpression<T>)
                    {
                        var clauseExpr = criteria as IPagingCriteriaExpression<T>;
                        // ordination by ascending expression
                        if (clauseExpr.OrderByAscendingExpr.AnyOrNotNull())
                        {
                            IOrderedQueryable<T> queryOrdered = null;
                            foreach (var ord in clauseExpr.OrderByAscendingExpr)
                            {
                                if (queryOrdered == null)
                                {
                                    ordered = true;
                                    queryOrdered = query.OrderBy(ord);
                                }
                                else
                                {
                                    queryOrdered = queryOrdered.ThenBy(ord);
                                }
                            }
                            query = queryOrdered ?? query;
                        }

                        // ordination by descending expression
                        if (clauseExpr.OrderByDescendingExpr.AnyOrNotNull())
                        {
                            IOrderedQueryable<T> queryOrdered = null;
                            foreach (var ord in clauseExpr.OrderByDescendingExpr)
                            {
                                if (queryOrdered == null)
                                {
                                    ordered = true;
                                    queryOrdered = query.OrderByDescending(ord);
                                }
                                else
                                {
                                    queryOrdered = queryOrdered.ThenByDescending(ord);
                                }
                            }
                            query = queryOrdered ?? query;
                        }
                    }

                    // ordination by string
                    if (criteria.OrderBy.AnyOrNotNull())
                    {
                        IOrderedQueryable<T> queryOrdered = null;
                        foreach (var ord in criteria.OrderBy)
                        {
                            if (queryOrdered == null)
                            {
                                ordered = true;
                                queryOrdered = query.OrderBy(ord);
                            }
                            else
                            {
                                queryOrdered = queryOrdered.ThenBy(ord);
                            }
                        }
                        query = queryOrdered ?? query;
                    }

                    // Paging
                    offset = criteria.Offset;
                    limit = criteria.Limit > 0 ? criteria.Limit : limit;
                }

                if (!ordered)
                {
                    query = SortByKey(query, GetKeyInfo());
                }

                // page index
                query = query.Skip(limit * offset);

                // number of records per page
                query = query.Take(limit);
            }

            if (criteria != null)
            {
                // navigation
                if (criteria is IPagingCriteriaExpression<T>)
                {
                    var clauseExpr = criteria as IPagingCriteriaExpression<T>;
                    // navigation by expression
                    if (clauseExpr.NavigationExpr.AnyOrNotNull())
                    {
                        foreach (var nav in clauseExpr.NavigationExpr)
                        {
                            query = query.Include(nav);
                        }
                    }
                }

                // navigation by string
                if (criteria.Navigation.AnyOrNotNull())
                {
                    foreach (var nav in criteria.Navigation)
                    {
                        query = query.Include(nav);
                    }
                }
            }

            return query;
        }

        protected TransactionScope CreateTransactionScope(bool isAggregate = false)
        {
            if (isAggregate || EnableReadUncommitedQuery)
            {
                return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted,
                    Timeout = TransactionManager.MaximumTimeout
                }, TransactionScopeAsyncFlowOption.Enabled);
            }
            return null;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Maximum amount returned in query
        /// </summary>
        protected int MaxQtyByQueryPage
        {
            get
            {
                return ConfigurationPropertiesHelper.MaxQtyByQueryPage;
            }
        }

        #endregion

        #region [ Supports ]

        PropertyInfo _keyInfo;
        /// <summary>
        /// 
        /// </summary>
        protected PropertyInfo GetKeyInfo()
        {
            if (_keyInfo == null)
            {
                _keyInfo = typeof(T).GetTypeInfo()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(x => x.GetCustomAttribute<KeyAttribute>() != null)
                    .FirstOrDefault();

                if (_keyInfo == null)
                {
                    throw new InvalidOperationException("Key property not found.");
                }
            }

            return _keyInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        protected IQueryable<T> GetDynamicFilter<TValue>(IQueryable<T> query, PropertyInfo key, TValue value)
        {
            var entityParameter = Expression.Parameter(typeof(T), "e");

            var lambda =
                Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Property(entityParameter, key),
                        ((value != null && value.GetType() == key.PropertyType) || typeof(TValue) == key.PropertyType)
                            ? Expression.Constant(value)
                                : Expression.Convert(Expression.Constant(value), key.PropertyType)),
                        entityParameter);

            return query.Where(lambda);
        }
        /// <summary>
        /// 
        /// </summary>
        protected IQueryable<T> SortByKey(IQueryable<T> query, PropertyInfo key)
        {
            try
            {
                Type t = typeof(T);
                var param = Expression.Parameter(t);

                return query.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        "OrderBy",
                        new Type[] { t, key.PropertyType },
                        query.Expression,
                        Expression.Quote(
                            Expression.Lambda(
                                Expression.Property(param, key),
                                param))
                    ));
            }
            catch (Exception) // Probably invalid input, you can catch specifics if you want
            {
                return query; // Return unsorted query
            }
        }

        #endregion
    }
}
