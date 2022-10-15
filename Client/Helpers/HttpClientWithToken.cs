namespace BlazorMovies.Client.Helpers
{
    public class HttpClientWithToken
    {

        public HttpClientWithToken(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient  _httpClient {get;}
    }

}
