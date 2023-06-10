using Microsoft.AspNetCore.Mvc;
using SPIAPI;
using SupportBot.Commands;
using SupportBot.Service;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SupportBot.Controllers
{
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        private readonly ISupportService _supportService;
        private readonly IConfiguration _configuration;

        private TelegramBotClient _botClient = Bot.GetTelegramBot();
        private CommandExecutor _commandExecutor = new CommandExecutor();

        public BotController(IConfiguration configuration, ISupportService supportService)
        {
            _configuration = configuration;
            _supportService = supportService;
        }

        [HttpPost]
        public async Task Post(Update update)
        {
            if (_supportService.token.TimeOfToken <= DateTime.UtcNow)
            {
                _supportService.token.TokenApi = null;
            }

            if (_supportService.token.TokenApi == null)
            {
                var telegram = await _supportService.LoginAsync(_configuration["SupportService:Username"], _configuration["SupportService:Password"]);
                if (telegram == null)
                {
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "A critical error has occurred. Try using the service at another time.");
                    return;
                }
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data == "call_support")
                {
                    var user = await _supportService.GetUserByTelegramIdAsync(update.CallbackQuery.Message.Chat.Id.ToString());
                    if (user == null) return;

                    if (user.TicketId == null)
                    {
                        var ticket = await _supportService.CreateTicket(user.Id, "None", "None");
                        if (ticket != null)
                        {
                            await _botClient.SendTextMessageAsync(user.TelegramId, "Вы успешно отправили заявку. Пожалуйста дождитесь ответа сотрудника");
                        }
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(user.TelegramId, "Вы уже отправляли заявку ранее. Пожалуйста дождитесь ответа сотрудника");
                    }
                }
            }

            if (update.Message == null)
                return;

            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                await _commandExecutor.GetUpdate(update, _supportService);

                var user = await _supportService.GetUserByTelegramIdAsync(update.Message.Chat.Id.ToString());
                if (user == null || user.TicketId == null) return;

                var ticket = await _supportService.GetTicketByIdAsync(user.TicketId);
                if (ticket == null) return;

                if (ticket.AssignedToUserId == null) return;

                var success = await _supportService.SendMessageToTicketAsync(ticket.Id, user.Id, update.Message.Text);
                if (success == null) return;

            }
        }

        [HttpGet]
        public string Get()
        {
            return "Telegram bot was started";
        }
    }
}
