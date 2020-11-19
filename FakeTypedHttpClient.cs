using System.Net.Http;

namespace HttpClientCheckExample
{
    public interface IFakeClient
    {
        string SomeApiRequest(string value);
    }
    
    public class FakeTypedHttpClient : IFakeClient
    {
        private readonly HttpClient _httpClient;
        
        public FakeTypedHttpClient(HttpClient httpClient) => _httpClient = httpClient;

        public string SomeApiRequest(string value)
        {
            return $"{value} that what you need!";
        }
    }
}