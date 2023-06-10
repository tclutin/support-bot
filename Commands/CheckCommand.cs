using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;
using SupportBot.Service;
using SPIAPI;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading;
using Telegram.Bot.Types.Enums;

namespace SupportBot.Commands
{
    public class CheckCommand : ICommand
    {
        public TelegramBotClient Client => Bot.GetTelegramBot();

        public string Name => "/check";

        public async Task Execute(Update update, ISupportService supportService)
        {
            long chatId = update.Message.Chat.Id;

            var user = await supportService.GetUserByTelegramIdAsync(chatId.ToString());
            if (user == null) return;

            if (user.TicketId == null)
            {
                await Client.SendTextMessageAsync(chatId, "В данный момент вы не имеете тикета.");
                return;
            }

            var ticket = await supportService.GetTicketByIdAsync(user.TicketId);
            if (ticket == null) return;

            await Client.SendTextMessageAsync(chatId,
                $"Ваш тикет:\nId: {ticket.Id}\nStatus: {ticket.Status}\nCreated: {ticket.CreatedDate}");
        }
    }
}
