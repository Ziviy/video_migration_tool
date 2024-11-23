namespace video_migration_tool.TelegramBot.Interfaces;

public interface IReceiverService
{
    public Task ReceiveAsync(CancellationToken stoppingToken);
}