namespace encouraging_bot.TelegramBot.Interfaces;

public interface IReceiverService
{
    public Task ReceiveAsync(CancellationToken stoppingToken);
}