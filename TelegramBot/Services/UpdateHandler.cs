using encouraging_bot.YouTube.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace encouraging_bot.TelegramBot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<IUpdateHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UpdateHandler(
        ILogger<IUpdateHandler> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (update.Message is null)
            return;
        
        _logger.LogInformation($"Received {update.Type} '{update.Message.Text}' in {update.Message.Chat.Id}");
        
        await botClient.SendTextMessageAsync(update.Message.Chat, "The video is being downloaded. Please wait", cancellationToken: cancellationToken);
        using var scope = _serviceProvider.CreateScope();
        var downloader = scope.ServiceProvider.GetRequiredService<IDownloader>();

        try
        {
            await downloader.Download(update.Message.Text);
            await botClient.SendTextMessageAsync(update.Message.Chat, "The video has been downloaded.", cancellationToken: cancellationToken);
        }
        catch(Exception e)
        {
            _logger.LogInformation("Error occurred while downloading video\n" + e);
            await botClient.SendTextMessageAsync(update.Message.Chat, "An error occurred while downloading video", cancellationToken: cancellationToken);
        }
       
    }
    
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);
    }
}