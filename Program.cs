using System.Collections.Immutable;
using video_migration_tool.Services.General;
using video_migration_tool.Services.General.Interfaces;
using video_migration_tool.Services.Interfaces;
using video_migration_tool.Services.VK;
using video_migration_tool.Services.YouTube;
using video_migration_tool.TelegramBot;
using video_migration_tool.TelegramBot.Interfaces;
using video_migration_tool.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("config.json")
    .AddEnvironmentVariables();

Config? configuration = builder.Configuration.Get<Config>();
ArgumentNullException.ThrowIfNull(configuration);

builder.Services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
    .AddTypedClient<ITelegramBotClient>(httpClient =>
    {
        TelegramBotClientOptions options = new(configuration.Telegram.Token);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();
builder.Services.AddScoped<IReceiverService, ReceiverService>();
builder.Services.AddScoped<IDownload, YouTubeDownloader>();
builder.Services.AddTransient<IUpload, VkService>();
builder.Services.AddSingleton<IGeneralServices, GeneralServices>();
builder.Services.AddTransient<IVkApi, VkApi>(provider =>
{
    var vkapi = new VkApi();
    vkapi.Authorize(new ApiAuthParams()
    {
        AccessToken = configuration.VK.Token,
        Settings = Settings.Video,
        ApplicationId = Convert.ToUInt64(configuration.VK.ApplicationId)
    });
    return vkapi;
});
builder.Services.AddHostedService<BackgroundBotService>();


var host = builder.Build();

host.Run();