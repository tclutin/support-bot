using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;
using SupportBot.Service;

namespace SupportBot.Commands
{
    public interface ICommand
    {
        public TelegramBotClient Client { get; }

        public string Name { get; }

        public async Task Execute(Update update, ISupportService supportService) { }
    }
}
