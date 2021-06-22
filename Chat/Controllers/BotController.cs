using Azure.Communication;
using Azure.Communication.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Chat.Controllers
{
    [Route("bot")]
    public class BotController : Controller
    {
        private readonly IChatAdminThreadStore _chatAdminThreadStore;
        private IUserTokenManager _userTokenManager;
        private string _botAcsId;
        private readonly string _botName;
        private readonly string _resourceConnectionString;
        private string _chatGatewayUrl;

        public BotController(
            IChatAdminThreadStore chatAdminThreadStore,
            IConfiguration configuration,
            IUserTokenManager userTokenManager)
        {
            _chatAdminThreadStore = chatAdminThreadStore;
            _userTokenManager = userTokenManager;
            _botAcsId = configuration["BotAcsId"];
            _botName = configuration["BotName"];
            _resourceConnectionString = configuration["ResourceConnectionString"];
            _chatGatewayUrl = Utils.ExtractApiChatGatewayUrl(_resourceConnectionString);
        }

        [HttpGet]
        public string GetBotId() => _botAcsId;

        [HttpPost("{threadId}")]
        public async Task<ActionResult> AddBotToThread(string threadId)
        {
            try
            {
                var chatThreadClient = await GetChatThreadClientAsync(threadId);

                var response = await chatThreadClient.AddParticipantAsync(
                    new ChatParticipant(new CommunicationUserIdentifier(_botAcsId))
                    {
                        DisplayName = _botName,
                        ShareHistoryTime = chatThreadClient.GetProperties().Value.CreatedOn
                    });
                Console.WriteLine($"Added bot to thread with status code: {response.Status}");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to add bot {_botAcsId} to thread {threadId} with exception: {e}");
                throw;
            }
        }

        [HttpDelete("{threadId}")]
        public async Task<ActionResult> RemoveBotFromThread(string threadId)
        {
            try
            {
                var chatThreadClient = await GetChatThreadClientAsync(threadId);
                var response = await chatThreadClient.RemoveParticipantAsync(new CommunicationUserIdentifier(_botAcsId));
                Console.WriteLine($"Removed the bot from the thread: {response.Status}");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remove bot {_botAcsId} to thread {threadId} with exception: {ex}");
                throw;
            }
        }

        private async Task<ChatThreadClient> GetChatThreadClientAsync(string threadId)
        {
            var moderator = _chatAdminThreadStore.Store[threadId];
            var moderatorToken = await _userTokenManager.GenerateTokenAsync(_resourceConnectionString, moderator);
            var chatClient = new ChatClient(
                new Uri(_chatGatewayUrl),
                new CommunicationTokenCredential(moderatorToken.Token));
            return chatClient.GetChatThreadClient(threadId);
        }
    }
}
