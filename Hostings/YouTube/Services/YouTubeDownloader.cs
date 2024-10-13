using encouraging_bot.Services.Converter;
using encouraging_bot.YouTube.Interfaces;
using FFMpegCore;
using FFMpegCore.Enums;
using Telegram.Bot.Types;
using VideoLibrary;
using File = System.IO.File;

namespace encouraging_bot.Hostings.YouTube.Services;

public class YouTubeDownloader : IDownloader
{
    private readonly string _dir;
    private readonly ILogger<IDownloader> _logger;
    private readonly IConverterService _converter;

    public YouTubeDownloader(
        ILogger<IDownloader> logger,
        IConverterService converter)
    {
        _logger = logger;
        _converter = converter;

        if (OperatingSystem.IsWindows())
            _dir = Directory.GetCurrentDirectory() + @"\Files\";
        else
            _dir = Directory.GetCurrentDirectory() + "/Files/";
    }

    public async Task Download(string url)
    {
        try
        {
            using var youTube = Client.For(new VideoLibrary.YouTube());
            var videoInfos = Client.For(VideoLibrary.YouTube.Default).GetAllVideosAsync(url).GetAwaiter().GetResult()
                .ToArray();

            var maxResolution = videoInfos.First(i => i.Resolution == videoInfos.Max(j => j.Resolution));
            var maxBitrate = videoInfos.First(i => i.AudioBitrate == videoInfos.Max(j => j.AudioBitrate));
            
            string videoName = "tmp-" + maxResolution.FullName;
            string audioName = "tmp-" + maxResolution.FullName + "audio." + maxBitrate.AudioFormat;

            var videoObj = new VideoObj(
                "tmp-" + maxResolution.FullName,
                "tmp-" + maxResolution.FullName + "audio." + maxBitrate.AudioFormat,
                _dir,
                maxResolution.FullName,
                maxBitrate
            );


            CheckDirectoryExists();
            _logger.LogInformation($"Downloading \"{maxResolution.Title}\" ({url}) to {_dir}");

            await File.WriteAllBytesAsync(_dir + videoName, await maxResolution.GetBytesAsync());
            await File.WriteAllBytesAsync(_dir + audioName, await maxBitrate.GetBytesAsync());

            // Combining audio and video files together
            _logger.LogInformation($"Converting video: " + videoName);
            await _converter.ConvertAsync(videoObj);


            _logger.LogInformation("Video has been downloaded. Service is free");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to download video: {url}", ex);
        }
    }

    private void CheckDirectoryExists()
    {
        if (!Directory.Exists(_dir))
        {
            Directory.CreateDirectory(_dir);
        }
    }
}