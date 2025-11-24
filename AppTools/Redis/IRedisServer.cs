using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppTools.Interface
{
    /// <summary>
    /// Redis服务接口
    /// </summary>
    public interface IRedisServer : IDisposable
    {
        #region 连接状态
        Task<bool> IsConnectedAsync();
        Task<Dictionary<string, string>> GetServerInfoAsync();
        #endregion

        #region 字符串操作
        Task<bool> SetStringAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetStringAsync<T>(string key);
        Task<bool> SetStringIfNotExistsAsync<T>(string key, T value, TimeSpan? expiry = null);
        #endregion

        #region 哈希操作
        Task<bool> SetHashAsync<T>(string key, string field, T value);
        Task<T> GetHashAsync<T>(string key, string field);
        Task<Dictionary<string, T>> GetHashAllAsync<T>(string key);
        Task<bool> DeleteHashAsync(string key, string field);
        #endregion

        #region 列表操作
        Task<long> ListLeftPushAsync<T>(string key, T value);
        Task<long> ListRightPushAsync<T>(string key, T value);
        Task<T> ListLeftPopAsync<T>(string key);
        Task<T> ListRightPopAsync<T>(string key);
        Task<List<T>> ListRangeAsync<T>(string key, long start = 0, long stop = -1);
        Task<long> ListLengthAsync(string key);
        #endregion

        #region 集合操作
        Task<bool> SetAddAsync<T>(string key, T value);
        Task<bool> SetRemoveAsync<T>(string key, T value);
        Task<HashSet<T>> SetMembersAsync<T>(string key);
        Task<bool> SetContainsAsync<T>(string key, T value);
        #endregion

        #region 有序集合操作
        Task<bool> SortedSetAddAsync<T>(string key, T value, double score);
        Task<bool> SortedSetRemoveAsync<T>(string key, T value);
        Task<List<T>> SortedSetRangeByRankAsync<T>(string key, long start = 0, long stop = -1, bool descending = false);
        Task<List<T>> SortedSetRangeByScoreAsync<T>(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity);
        #endregion

        #region 键操作
        Task<bool> KeyExistsAsync(string key);
        Task<bool> KeyDeleteAsync(string key);
        Task<bool> KeyExpireAsync(string key, TimeSpan expiry);
        Task<TimeSpan?> KeyTimeToLiveAsync(string key);
        Task<List<string>> KeySearchAsync(string pattern);
        Task<long> KeyDeleteByPatternAsync(string pattern);
        #endregion

        #region 发布订阅
        Task<long> PublishAsync<T>(string channel, T message);
        Task SubscribeAsync<T>(string channel, Action<string, T> handler);
        Task UnsubscribeAsync(string channel);
        #endregion

        #region 事务操作
        IRedisTransaction CreateTransaction();
        #endregion 
    }

    /// <summary>
    /// Redis事务接口
    /// </summary>
    public interface IRedisTransaction
    {
        IRedisTransaction SetString<T>(string key, T value, TimeSpan? expiry = null);
        Task<bool> ExecuteAsync();
    }
}