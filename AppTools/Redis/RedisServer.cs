using AppTools.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppTools.Serialize.Server
{
    public class RedisServer : IRedisServer
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly RedisOptions _options;
        private readonly JsonSerializerSettings _jsonSettings;
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RedisServer(RedisOptions options = null)
        {

            _options = options ?? new RedisOptions();
            _jsonSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            try
            {
                var config = ConfigurationOptions.Parse(_options.ConnectionString);
                config.ConnectTimeout = _options.ConnectTimeout * 1000;
                config.SyncTimeout = _options.SyncTimeout * 1000;
                config.AllowAdmin = _options.AllowAdmin;

                _redis = ConnectionMultiplexer.Connect(config);
                _database = _redis.GetDatabase();

                // 注册连接失败事件
                _redis.ConnectionFailed += (sender, args) =>
                {
                    Console.WriteLine($"Redis连接失败: {args.Exception?.Message}");
                };

                _redis.ConnectionRestored += (sender, args) =>
                {
                    Console.WriteLine("Redis连接已恢复");
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Redis连接失败: {ex.Message}", ex);
            }
        }

        #region 连接状态
        public async Task<bool> IsConnectedAsync()
        {
            try
            {
                await _database.PingAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Dictionary<string, string>> GetServerInfoAsync()
        {
            var result = new Dictionary<string, string>();

            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());

                // 只获取基本信息
                result["connected"] = "true";
                result["server_type"] = server.ServerType.ToString();
                result["version"] = server.Version?.ToString() ?? "unknown";
                result["is_connected"] = server.IsConnected.ToString();

                // 获取一些基本的统计信息
                var infoResult = await server.ExecuteAsync("INFO", "stats");
                if (infoResult != null)
                {
                    var infoText = infoResult.ToString();
                    var lines = infoText.Split('\n');

                    foreach (var line in lines)
                    {
                        if (line.Contains("instantaneous_ops_per_sec"))
                        {
                            var parts = line.Split(':');
                            if (parts.Length == 2)
                                result["ops_per_sec"] = parts[1].Trim();
                        }
                        else if (line.Contains("used_memory"))
                        {
                            var parts = line.Split(':');
                            if (parts.Length == 2)
                                result["used_memory"] = parts[1].Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result["error"] = ex.Message;
            }

            return result;
        }
        #endregion

        #region 字符串操作
        public async Task<bool> SetStringAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.StringSetAsync(fullKey, jsonValue, expiry);
        }

        public async Task<T> GetStringAsync<T>(string key)
        {
            var fullKey = GetFullKey(key);
            var value = await _database.StringGetAsync(fullKey);
            return value.HasValue ? Deserialize<T>(value) : default(T);
        }

        public async Task<bool> SetStringIfNotExistsAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.StringSetAsync(fullKey, jsonValue, expiry, When.NotExists);
        }
        #endregion

        #region 哈希操作
        public async Task<bool> SetHashAsync<T>(string key, string field, T value)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.HashSetAsync(fullKey, field, jsonValue);
        }

        public async Task<T> GetHashAsync<T>(string key, string field)
        {
            var fullKey = GetFullKey(key);
            var value = await _database.HashGetAsync(fullKey, field);
            return value.HasValue ? Deserialize<T>(value) : default(T);
        }

        public async Task<Dictionary<string, T>> GetHashAllAsync<T>(string key)
        {
            var fullKey = GetFullKey(key);
            var entries = await _database.HashGetAllAsync(fullKey);

            var result = new Dictionary<string, T>();
            foreach (var entry in entries)
            {
                result[entry.Name] = Deserialize<T>(entry.Value);
            }

            return result;
        }

        public async Task<bool> DeleteHashAsync(string key, string field)
        {
            var fullKey = GetFullKey(key);
            return await _database.HashDeleteAsync(fullKey, field);
        }
        #endregion

        #region 列表操作
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.ListLeftPushAsync(fullKey, jsonValue);
        }

        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.ListRightPushAsync(fullKey, jsonValue);
        }

        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            var fullKey = GetFullKey(key);
            var value = await _database.ListLeftPopAsync(fullKey);
            return value.HasValue ? Deserialize<T>(value) : default(T);
        }

        public async Task<T> ListRightPopAsync<T>(string key)
        {
            var fullKey = GetFullKey(key);
            var value = await _database.ListRightPopAsync(fullKey);
            return value.HasValue ? Deserialize<T>(value) : default(T);
        }

        public async Task<List<T>> ListRangeAsync<T>(string key, long start = 0, long stop = -1)
        {
            var fullKey = GetFullKey(key);
            var values = await _database.ListRangeAsync(fullKey, start, stop);
            return values.Select(v => Deserialize<T>(v)).ToList();
        }

        public async Task<long> ListLengthAsync(string key)
        {
            var fullKey = GetFullKey(key);
            return await _database.ListLengthAsync(fullKey);
        }
        #endregion

        #region 集合操作
        public async Task<bool> SetAddAsync<T>(string key, T value)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.SetAddAsync(fullKey, jsonValue);
        }

        public async Task<bool> SetRemoveAsync<T>(string key, T value)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.SetRemoveAsync(fullKey, jsonValue);
        }

        public async Task<HashSet<T>> SetMembersAsync<T>(string key)
        {
            var fullKey = GetFullKey(key);
            var values = await _database.SetMembersAsync(fullKey);
            return new HashSet<T>(values.Select(v => Deserialize<T>(v)));
        }

        public async Task<bool> SetContainsAsync<T>(string key, T value)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.SetContainsAsync(fullKey, jsonValue);
        }
        #endregion

        #region 有序集合操作
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.SortedSetAddAsync(fullKey, jsonValue, score);
        }

        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            var fullKey = GetFullKey(key);
            var jsonValue = Serialize(value);
            return await _database.SortedSetRemoveAsync(fullKey, jsonValue);
        }

        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key, long start = 0, long stop = -1, bool descending = false)
        {
            var fullKey = GetFullKey(key);
            var order = descending ? Order.Descending : Order.Ascending;
            var values = await _database.SortedSetRangeByRankAsync(fullKey, start, stop, order);
            return values.Select(v => Deserialize<T>(v)).ToList();
        }

        public async Task<List<T>> SortedSetRangeByScoreAsync<T>(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity)
        {
            var fullKey = GetFullKey(key);
            var values = await _database.SortedSetRangeByScoreAsync(fullKey, start, stop);
            return values.Select(v => Deserialize<T>(v)).ToList();
        }
        #endregion

        #region 键操作
        public async Task<bool> KeyExistsAsync(string key)
        {
            var fullKey = GetFullKey(key);
            return await _database.KeyExistsAsync(fullKey);
        }

        public async Task<bool> KeyDeleteAsync(string key)
        {
            var fullKey = GetFullKey(key);
            return await _database.KeyDeleteAsync(fullKey);
        }

        public async Task<bool> KeyExpireAsync(string key, TimeSpan expiry)
        {
            var fullKey = GetFullKey(key);
            return await _database.KeyExpireAsync(fullKey, expiry);
        }

        public async Task<TimeSpan?> KeyTimeToLiveAsync(string key)
        {
            var fullKey = GetFullKey(key);
            return await _database.KeyTimeToLiveAsync(fullKey);
        }

        public async Task<List<string>> KeySearchAsync(string pattern)
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{_options.InstanceName}{pattern}");
            return keys.Select(k => k.ToString().Replace(_options.InstanceName, "")).ToList();
        }

        public async Task<long> KeyDeleteByPatternAsync(string pattern)
        {
            var keys = await KeySearchAsync(pattern);
            if (!keys.Any()) return 0;

            var fullKeys = keys.Select(k => (RedisKey)GetFullKey(k)).ToArray();
            return await _database.KeyDeleteAsync(fullKeys);
        }
        #endregion

        #region 发布订阅
        public async Task<long> PublishAsync<T>(string channel, T message)
        {
            var jsonMessage = Serialize(message);
            var subscriber = _redis.GetSubscriber();
            return await subscriber.PublishAsync(channel, jsonMessage);
        }

        public async Task SubscribeAsync<T>(string channel, Action<string, T> handler)
        {
            var subscriber = _redis.GetSubscriber();
            await subscriber.SubscribeAsync(channel, (redisChannel, value) =>
            {
                var message = Deserialize<T>(value);
                handler(channel, message);
            });
        }

        public async Task UnsubscribeAsync(string channel)
        {
            var subscriber = _redis.GetSubscriber();
            await subscriber.UnsubscribeAsync(channel);
        }
        #endregion

        #region 事务操作
        public IRedisTransaction CreateTransaction()
        {
            return new RedisTransaction(_database.CreateTransaction(), _options.InstanceName, _jsonSettings);
        }
        #endregion

        #region 辅助方法
        private string GetFullKey(string key)
        {
            return $"{_options.InstanceName}{key}";
        }

        private string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _jsonSettings);
        }

        private T Deserialize<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value, _jsonSettings);
        }
        #endregion

        #region Dispose模式
        public void Dispose()
        {
            if (!_disposed)
            {
                _redis?.Dispose();
                _disposed = true;
            }
        }
        #endregion
    }

    /// <summary>
    /// Redis事务实现
    /// </summary>
    internal class RedisTransaction : IRedisTransaction
    {
        private readonly ITransaction _transaction;
        private readonly string _instanceName;
        private readonly JsonSerializerSettings _jsonSettings;

        public RedisTransaction(ITransaction transaction, string instanceName, JsonSerializerSettings jsonSettings)
        {
            _transaction = transaction;
            _instanceName = instanceName;
            _jsonSettings = jsonSettings;
        }
        
        public IRedisTransaction SetString<T>(string key, T value, TimeSpan? expiry = null)
        {
            var fullKey = $"{_instanceName}{key}";
            var jsonValue = JsonConvert.SerializeObject(value, _jsonSettings);
            _transaction.StringSetAsync(fullKey, jsonValue, expiry);
            return this;
        }

        public async Task<bool> ExecuteAsync()
        {
            return await _transaction.ExecuteAsync();
        }
    }
}