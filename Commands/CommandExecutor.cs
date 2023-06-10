
using SupportBot.Service;
using Telegram.Bot.Types;

namespace SupportBot.Commands
{
    public class CommandExecutor
    {

        private List<ICommand> commands;

        public CommandExecutor()
        {
            commands = new List<ICommand>
            {
                new StartCommand(),
                new LookCommand(),
            };
        }

        public async Task GetUpdate(Update update, ISupportService supportService)
        {
            Telegram.Bot.Types.Message msg = update.Message;
            if (msg.Text == null) 
                return;

            foreach (var command in commands)
            {
                if (command.Name == msg.Text)
                {
                    await command.Execute(update, supportService);
                }
            }
        }
    }
}
