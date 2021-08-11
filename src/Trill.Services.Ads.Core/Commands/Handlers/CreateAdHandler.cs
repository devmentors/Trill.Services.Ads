using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Trill.Services.Ads.Core.Clients;
using Trill.Services.Ads.Core.Clients.Requests;
using Trill.Services.Ads.Core.Domain;
using Trill.Services.Ads.Core.Domain.Exceptions;
using Trill.Services.Ads.Core.Events;

namespace Trill.Services.Ads.Core.Commands.Handlers
{
    internal sealed class CreateAdHandler : ICommandHandler<CreateAd>
    {
        private readonly IAdRepository _adRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IStoryApiClient _storyApiClient;

        public CreateAdHandler(IAdRepository adRepository, IMessageBroker messageBroker, IStoryApiClient storyApiClient)
        {
            _adRepository = adRepository;
            _messageBroker = messageBroker;
            _storyApiClient = storyApiClient;
        }

        public async Task HandleAsync(CreateAd command)
        {
            var ad = new Ad(command.AdId, command.UserId, command.Header, command.Content, command.Tags,
                AdState.New, command.From, command.To, DateTime.UtcNow);
            
            // TO BE REFACTORED FURTHER
            var storyId = await _storyApiClient.SendStoryAsync(new SendStoryRequest
            {
                UserId = ad.UserId,
                Title = ad.Header,
                Text = ad.Content,
                Tags = ad.Tags,
                Highlighted = true,
                VisibleFrom = ad.From,
                VisibleTo = ad.To
            });
    
            if (storyId is null)
            {
                throw new CannotCreateAdAException(command.AdId);
            }
    
            ad.SetStoryId(storyId.Value);
            await _adRepository.AddAsync(ad);
            await _messageBroker.PublishAsync(new AdCreated(ad.Id));
        }
    }
}