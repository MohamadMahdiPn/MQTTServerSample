namespace MQTTServerSample.Application.Contracts.Redis;

public interface IRedisService
{
    Task SaveToCashAsync(string key, string value, int expirationInSeconds = 1);
    Task SaveToCashAsync<TEntity>(string key, TEntity value, int expirationInHours = 1);
    Task<string> RetrieveFromCashAsync(string key);
    Task<TEntity> RetrieveFromCashAsync<TEntity>(string key);
}
