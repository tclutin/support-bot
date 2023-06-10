using SupportBot.Service;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace SupportBot.Commands
{
    public class StartCommand : ICommand
    {
        public TelegramBotClient Client => Bot.GetTelegramBot();

        public string Name => "/start";

        public async Task Execute(Update update, ISupportService supportService)
        {
        }
    }
}
