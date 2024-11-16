using Microsoft.Extensions.Caching.Distributed;
using MQTTServerSample.Application.Contracts.Redis;
using Newtonsoft.Json;

namespace MQTTServerSample.Persistence.RedisRepository;

public class RedisDataService : IRedisService
{
    #region Constructor
    private readonly IDistributedCache _distributedCache;

    public RedisDataService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;


    }
    #endregion


    #region SaveToCash
    public async Task SaveToCashAsync(string key, string value, int expirationInSeconds = 5400)
    {
        try
        {
            await _distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(expirationInSeconds)
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task SaveToCashAsync<TEntity>(string key, TEntity value, int expirationInHours = 1)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(expirationInHours)
            });
        }
        catch
        {

        }
    }
    #endregion

    #region RetrieveFromCash
    public async Task<string> RetrieveFromCashAsync(string key)
    {
        try
        {
            var value = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public async Task<TEntity> RetrieveFromCashAsync<TEntity>(string key)
    {
        try
        {
            var value = await _distributedCache.GetStringAsync(key);
            return JsonConvert.DeserializeObject<TEntity>(value);
        }
        catch
        {
            return default;
        }
    }
    #endregion
}
