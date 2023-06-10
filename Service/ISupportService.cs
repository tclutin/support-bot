namespace SupportBot.Service
{
    public interface ISupportService
    {
        Token token { get; set; }
        Task<AuthResponse?> LoginAsync(string username, string password);
        Task<IEnumerable<Ticket>?> GetOpenTicketsAsync();
        Task<SuccessfulMessage?> CreateTelegramUserAsync(string name, string telegramid);
        Task<User?> GetUserByTelegramIdAsync(string telegramId);
        Task<Ticket?> GetTicketByIdAsync(Guid? ticketId);
        Task<SuccessfulMessage?> SendMessageToTicketAsync(Guid ticketId, Guid senderId, string text);
        Task<SuccessfulMessage?> CreateTicket(Guid userId, string title, string description);
        Task<SuccessfulMessage?> AssignByTicketAsync(Guid ticketId, Guid userId);
        Task<SuccessfulMessage?> CloseTicketAsync(Guid ticketId);
    }
}
