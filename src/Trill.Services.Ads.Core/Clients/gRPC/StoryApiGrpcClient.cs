using System.Threading.Tasks;
using Grpc.Net.Client;
using Trill.Services.Ads.Core.Clients.Requests;

namespace Trill.Services.Ads.Core.Clients.gRPC
{
    internal class StoryApiGrpcClient : IStoryApiClient
    {
        private readonly StoryService.StoryServiceClient _client;
        
        public StoryApiGrpcClient(GrpcChannel channel)
        {
            _client = new StoryService.StoryServiceClient(channel);
        }

        public async Task<long?> SendStoryAsync(SendStoryRequest request)
        {
            var response = await _client.SendStoryAsync(new SendStoryCommand
            {
                UserId = request.UserId.ToString(),
                Title = request.Title,
                Text = request.Text,
                Tags = {request.Tags},
                Highlighted = request.Highlighted,
                VisibleFrom = request.VisibleFrom.ToString("u"),
                VisibleTo = request.VisibleTo.ToString("u")
            });

            return response?.Id;
        }
    }
}