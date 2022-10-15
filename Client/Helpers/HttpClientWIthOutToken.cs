namespace BlazorMovies.Client.Helpers
{
    public class HttpClientWIthOutToken
    {
        public HttpClient _httpClient { get;}
        public HttpClientWIthOutToken(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
