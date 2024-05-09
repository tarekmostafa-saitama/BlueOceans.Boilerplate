namespace Infrastructure.Caching;

public class CacheSettings
{
    public bool UseDistributedCache { get; set; }
    public bool UseRedis { get; set; }
    public string RedisUrl { get; set; }
}