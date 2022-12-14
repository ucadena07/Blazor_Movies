using System.Text;
using System.Text.Json;

namespace BlazorMovies.Client.Helpers
{
    public class HttpService : IHttpService
    {
        private readonly HttpClientWithToken _httpClientWithToken;
        private readonly HttpClientWIthOutToken _httpClientWithOutToken;
        private JsonSerializerOptions defaultJsonSerializerOptions => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        public HttpService(HttpClientWithToken httpClientWithToken, HttpClientWIthOutToken httpClientWithOutToken)
        {
            _httpClientWithToken = httpClientWithToken;
            _httpClientWithOutToken = httpClientWithOutToken;
        }

        private HttpClient GetHttpClient(bool includeToken = true)
        {
            if (includeToken)
                return _httpClientWithToken._httpClient;
            else
                return _httpClientWithOutToken._httpClient;
        }


        public async Task<HttpResponseWrapper<T>> Get<T>(string url, bool includeToken = true)
        {
            var _httpClient = GetHttpClient(includeToken);
            var responseHttp = await _httpClient.GetAsync(url);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHttp, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(response, true, responseHttp);
            }
            else
            {
                return new HttpResponseWrapper<T>(default, false, responseHttp);
            }
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            var _httpClient = GetHttpClient();
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            var _httpClient = GetHttpClient();
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }
        public async Task<HttpResponseWrapper<object>> Delete<T>(string url)
        {
            var _httpClient = GetHttpClient();
            var responseHttp = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, responseHttp.IsSuccessStatusCode, responseHttp);
        }
        public async Task<HttpResponseWrapper<TResponse>> Post<T,TResponse>(string url, T data, bool includeToken = true)
        {
            var _httpClient = GetHttpClient(includeToken);
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialize = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<TResponse>(responseDeserialize, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }
      
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }

     
    }
}
