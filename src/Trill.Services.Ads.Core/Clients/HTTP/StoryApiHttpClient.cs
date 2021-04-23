using System.Threading.Tasks;
using Trill.Services.Ads.Core.Clients.Requests;

namespace Trill.Services.Ads.Core.Clients.HTTP
{
    internal sealed class StoryApiHttpClient : IStoryApiClient
    {
        public async Task<long?> SendStoryAsync(SendStoryRequest request)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}