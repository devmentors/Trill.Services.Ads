using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Convey.HTTP;
using Trill.Services.Ads.Core.Clients.Requests;

namespace Trill.Services.Ads.Core.Clients.HTTP
{
    internal sealed class StoryApiHttpClient : IStoryApiClient
    {
        private readonly IHttpClient _client;
        private readonly string _url;

        public StoryApiHttpClient(IHttpClient client, HttpClientOptions options)
        {
            _client = client;
            _url = options.Services["stories"];
        }

        public async Task<long?> SendStoryAsync(SendStoryRequest request)
        {
            var response = await _client.PostAsync($"{_url}/stories", request);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var location = response.Headers.Location.ToString();
            var storyId = location.Split("/").Last();

            return long.Parse(storyId);
        }
    }
}