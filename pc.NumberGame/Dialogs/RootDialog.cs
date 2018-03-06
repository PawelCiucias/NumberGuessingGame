using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace pc.NumberGame.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        static Random rnd = new Random(DateTime.Now.Second * DateTime.Now.Millisecond);

        int number = 0;
        int count = 0;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            number = rnd.Next(1, 11);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            count++;
            int guess = -1;

            var message = await result as IMessageActivity;
            var response = context.MakeMessage();
         
            if (int.TryParse(message.Text, out guess) && guess > 0 && guess < 11)
            {
                if (guess < number)
                    response.Text = $"your guess of '{guess}' was too low";
                else if (guess > number)
                    response.Text = $"your guess of '{guess}' was too high";
                else
                {
                    response.Text = $"your guess of '{guess}' was correct, it took {count} guess's to get it right.\n\nFeel free to play again";
                  


                    //Pick next number to guess
                    number = rnd.Next(1, 11);

                    //reset counter
                    count = 0;
                }
            }
            else
            {
                response.Text = $"I am sorry '{ message.Text}' is not a valid integer between 1 and 10 inclusively";
            }

            await context.PostAsync(response);

            context.Wait(MessageReceivedAsync);
        }
    }
}