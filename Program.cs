using encouraging_bot;
using encouraging_bot.Hostings.YouTube.Services;
using encouraging_bot.TelegramBot;
using encouraging_bot.TelegramBot.Interfaces;
using encouraging_bot.TelegramBot.Services;
using encouraging_bot.YouTube.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using VideoLibrary;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddCommandLine(args);

TelegramConfig? botConfiguration = builder.Configuration.GetSection("Telegram").Get<TelegramConfig>();
ArgumentNullException.ThrowIfNull(botConfiguration);

builder.Services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
    .AddTypedClient<ITelegramBotClient>(httpClient =>
    {
        TelegramBotClientOptions options = new(botConfiguration.Token);
        return new TelegramBotClient(options, httpClient);
    });


builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();
builder.Services.AddScoped<IReceiverService, ReceiverService>();
builder.Services.AddSingleton<VideoClient>();
builder.Services.AddScoped<IDownloader, YouTubeDownloader>();


builder.Services.AddHostedService<BackgroundBotService>();

var host = builder.Build();

host.Run();