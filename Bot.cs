using SPIAPI;
using Telegram.Bot;

namespace SupportBot
{
    public class Bot
    {
        private static TelegramBotClient client { get; set; }

        public static TelegramBotClient GetTelegramBot()
        {
            if (client != null)
            {
                return client;
            }
            client = new TelegramBotClient("6198831431:AAHOF8x89rppG27arCEQwwWXtYBDG6OPN_4");
            return client;
        }
    }
}
