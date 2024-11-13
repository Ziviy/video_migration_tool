using video_migration_tool.Services.Interfaces;
using video_migration_tool.Models;
using video_migration_tool.Services.General.Interfaces;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace video_migration_tool.Services.YouTube;

public class YouTubeDownloader : IDownload
{
    private readonly string _dir;
    private readonly ILogger<IDownload> _logger;

    public YouTubeDownloader(
        ILogger<IDownload> logger)
    {
        _logger = logger;

        if (OperatingSystem.IsWindows())
            _dir = Directory.GetCurrentDirectory() + @"\Files\";
        else
            _dir = Directory.GetCurrentDirectory() + "/Files/";
    }

    public async Task<IntegrationFileInfo> Download(string url)
    {
        try
        {
            var youTube = new YoutubeClient();
            var streamManifest = await youTube.Videos.Streams.GetManifestAsync(url);
            var videoInfo = await youTube.Videos.GetAsync(url);
            var videoStream = streamManifest
                .GetVideoOnlyStreams()
                .GetWithHighestVideoQuality();
            var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            var filePath = $"{_dir}{videoInfo.Title}.{videoStream.Container}";
            var fileName = $"{videoInfo.Title}.{videoStream.Container}.mp4";
            CheckDirectoryExists();
            _logger.LogInformation($"Downloading \"{videoInfo.Title}\" ({url}) to {_dir}");

            await youTube.Videos.DownloadAsync(url, filePath);

            _logger.LogInformation("Converting video: " + "test");


            _logger.LogInformation("Video has been downloaded. Service is free");
            return new IntegrationFileInfo(fileName, filePath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to download video: {url}", ex);
        }


    }

    private void CheckDirectoryExists()
    {
        if (!Directory.Exists(_dir)) Directory.CreateDirectory(_dir);
    }
}