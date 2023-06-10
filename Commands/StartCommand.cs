using SupportBot.Service;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace SupportBot.Commands
{
    public class StartCommand : ICommand
    {
        public TelegramBotClient Client => Bot.GetTelegramBot();

        public string Name => "/start";

        public async Task Execute(Update update, ISupportService supportService)
        {
            long chatId = update.Message.Chat.Id;

            var user = await supportService.GetUserByTelegramIdAsync(chatId.ToString());
            if (user == null)
            {
                await supportService.CreateTelegramUserAsync(update.Message.Chat.Username, chatId.ToString());
            }

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                 InlineKeyboardButton.WithCallbackData("Вызвать сотрудника", "call_support"),
                }
                });

            await Client.SendTextMessageAsync(chatId,
                       "Добро пожаловать. Вы попали в сервис технической поддержки. Нажмите кнопку, чтобы вызвать сотрудника поддержки для решения ваших вопросов.",
            replyMarkup: keyboard);
        }
    }
}
