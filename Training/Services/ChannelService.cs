using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Carts;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using commercetools.Sdk.Api.Models.Channels;

namespace Training.Services
{
    public class ChannelService
    {
        private readonly IClient _client;
        private readonly string _projectKey;

        public ChannelService(IClient client, string projectKey)
        {
            _client = client;
            _projectKey = projectKey;
        }

        /// <summary>
        /// GET a channel by id
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<IChannel> GetChannelById(string channelId)
        {
            IChannel channel = await _client.WithProject(_projectKey)
                .Channels()
                .WithId(channelId)
                .Get()
                .ExecuteAsync();
            
            return channel;
        }
    }
}