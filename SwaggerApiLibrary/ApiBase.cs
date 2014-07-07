using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Swagger
{
    #region Base service response definition
    public class IBaseResponse<T>
    {
        public bool error { get; set; }
        public string error_message { get; set; }
        public int error_code { get; set; }
        public T results { get; set; }
        public bool mocked { get; set; }
        public string execution_time;
    }
    #endregion

    public static class Api
    {
        private static string ApiKey = null;
        private static string Mocked = "false";

        public static void Initialize(string ApiKey, bool mocked = false)
        {
            Api.ApiKey = ApiKey;
            Api.Mocked = mocked.ToString().ToLower();
        }

        #region Base HTTP function definition
        public static HttpClient CreateClient(string BasePath, string contentType)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BasePath);
            httpClient.DefaultRequestHeaders.Add("X-API-TOKEN", Api.ApiKey);
            httpClient.DefaultRequestHeaders.Add("X-API-MOCK", Api.Mocked);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            return httpClient;
        }

        public static async Task<T> PostForm<T>(string basePath, string url, Dictionary<string, string> content, string contentType = "application/x-www-form-urlencoded")
        {
            var httpClient = CreateClient(basePath, contentType);
            var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(content));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
        public static async Task<T> PostRaw<T>(string basePath, string url, string content, string contentType = "application/json")
        {
            var httpClient = CreateClient(basePath, contentType);
            var response = await httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, contentType));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
        public static async Task<T> PutForm<T>(string basePath, string url, Dictionary<string, string> content, string contentType = "application/x-www-form-urlencoded")
        {
            var httpClient = CreateClient(basePath, contentType);
            var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(content));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
        public static async Task<T> PutRaw<T>(string basePath, string url, string content, string contentType = "application/json")
        {
            var httpClient = CreateClient(basePath, contentType);
            var response = await httpClient.PutAsync(url, new StringContent(content, Encoding.UTF8, contentType));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> Get<T>(string basePath, string url, string contentType = "application/json")
        {
            var httpClient = CreateClient(basePath, contentType);
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> Delete<T>(string basePath, string url, string contentType = "application/json")
        {
            var httpClient = CreateClient(basePath, contentType);
            var response = await httpClient.DeleteAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        #endregion

    }
}
