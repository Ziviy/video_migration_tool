using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using video_migration_tool.Models;
using video_migration_tool.Services.General.Interfaces;
using video_migration_tool.Services.Interfaces;

namespace video_migration_tool.TelegramBot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<IUpdateHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUpload _uploader;

    public UpdateHandler(
        ILogger<IUpdateHandler> logger,
        IServiceProvider serviceProvider,
        IUpload uploader)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _uploader = uploader;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (update.Message is null)
            return;

        _logger.LogInformation($"Received {update.Type} '{update.Message.Text}' in {update.Message.Chat.Id}");

        await botClient.SendMessage(update.Message.Chat, "The video is being downloaded. Please wait",
            cancellationToken: cancellationToken);
        using var scope = _serviceProvider.CreateScope();
        var downloader = scope.ServiceProvider.GetRequiredService<IDownload>();

        try
        {
            var integrationFileInfo = await downloader.Download(update.Message.Text);
            await botClient.SendMessage(update.Message.Chat, "The video has been downloaded.",
                cancellationToken: cancellationToken);
            await _uploader.UploadAsync(integrationFileInfo);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Error occurred while downloading video\n" + e);
            await botClient.SendMessage(update.Message.Chat, "An error occurred while downloading video",
                cancellationToken: cancellationToken);
        }

    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);
    }

}