namespace Eciton.Application.Abstractions;
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T data, int seconds = 300);
    Task<bool> IsExistsAsync(string key);
    void Delete(string key);
}
