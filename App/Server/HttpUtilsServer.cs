using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Server
{
    public static class HttpUtilsServer
    {
        public static readonly HttpClient _httpClient;

        /// <summary>
        /// 静态构造函数：初始化 HttpClient
        /// </summary>
        static HttpUtilsServer()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                AddHeaders(request, headers);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
             
                throw new HttpRequestException($"GET请求失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        public static async Task<string> PostAsync(
        string url,
        object data,
        string contentType = "application/json",
        Dictionary<string, string> headers = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                AddHeaders(request, headers);

                string requestBody;

                // 🔥 如果 data 已经是 string（例如你传入 jsonData）
                if (data is string s)
                {
                    requestBody = s;   // ⬅️ 不再序列化
                }
                else
                {
                    // 🔥 data 是对象，才序列化
                    requestBody = JsonConvert.SerializeObject(data);
                }

                request.Content = new StringContent(requestBody, Encoding.UTF8, contentType);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"POST请求失败: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// 添加请求头
        /// </summary>
        private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}