using encouraging_bot.TelegramBot.Interfaces;

namespace encouraging_bot.TelegramBot.Services;

public class BackgroundBotService : BackgroundService
{
    private readonly ILogger<BackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundBotService(
        ILogger<BackgroundService> logger,
        IServiceProvider serviceProvider
    )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<IReceiverService>();
                await receiver.ReceiveAsync(stoppingToken);
            }

            catch (Exception ex)
            {
                _logger.LogError("Background service failed with exception: {Exception}", ex);

                // Cooldown if something goes wrong
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}