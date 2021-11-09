//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Infrastructure.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class RedisObjectCacheExtensions
    {
        private static readonly ILoggingService _logger;

#pragma warning disable S3963 // "static" fields should be initialized inline
        static RedisObjectCacheExtensions()
        {
            _logger = LoggingService.GetLoggingService();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static async Task<T> GetRedisObjectAsync<T>(this IDistributedCache cache, string key, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            try
            {
                string value = await cache.GetStringAsync(key, token);
                if (!value.HasValue())
                {
                    return default;
                }
                return value.ToDeserialize<T>(jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return default;
        }

        public static async Task SetRedisObjectAsync<T>(this IDistributedCache cache, string key, T value, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (value == null)
                return;
            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                await cache.SetRedisStringAsync(key, result, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetRedisObjectAsync<T>(this IDistributedCache cache, string key, T value, int minutes, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (value == null)
                return;
            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                await cache.SetRedisStringAsync(key, result, minutes, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static async Task SetRedisObjectAsync(this IDistributedCache cache, string key, string value, DateTimeOffset time, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken token = default)
        {
            if (value == null)
                return;
            try
            {
                string result = value.ToSerialize(jsonSerializerSettings);
                await cache.SetRedisStringAsync(key, result, time, token);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}