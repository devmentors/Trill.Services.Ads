using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Trill.Services.Ads.Core.Domain;
using Trill.Services.Ads.Core.Events;

namespace Trill.Services.Ads.Core.Commands.Handlers
{
    internal sealed class CreateAdHandler : ICommandHandler<CreateAd>
    {
        private readonly IAdRepository _adRepository;
        private readonly IMessageBroker _messageBroker;

        public CreateAdHandler(IAdRepository adRepository, IMessageBroker messageBroker)
        {
            _adRepository = adRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateAd command)
        {
            var ad = new Ad(command.AdId, command.UserId, command.Header, command.Content, command.Tags,
                AdState.New, command.From, command.To, DateTime.UtcNow);
            await _adRepository.AddAsync(ad);
            await _messageBroker.PublishAsync(new AdCreated(ad.Id));
        }
    }
}