using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace pc.NumberGame
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            else
                await HandleSystemMessage(activity);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed Use Activity.MembersAdded and     
                // Activity.MembersRemoved and Activity.Action for info Not available in all channels
                if (message.MembersAdded.Count > 0)
                {
                    var connector = new ConnectorClient(new System.Uri(message.ServiceUrl));

                    foreach (var member in message.MembersAdded)
                    {
                        // if the bot is added, then 
                        if (member.Id == message.Recipient.Id)
                        {
                            var conversationId = message.Conversation.Id;
                            var activityId = message.Id;
                            var replyActivity = message.CreateReply("Hi I’m the Guess a number bot, please guess a number between 0 and 10.");

                            await connector.Conversations.ReplyToActivityWithHttpMessagesAsync(conversationId, activityId, replyActivity);
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}