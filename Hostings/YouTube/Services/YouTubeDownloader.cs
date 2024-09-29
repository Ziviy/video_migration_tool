using encouraging_bot.YouTube.Interfaces;
using VideoLibrary;

namespace encouraging_bot.Hostings.YouTube.Services;

public class YouTubeDownloader : IDownloader
{
    private readonly string _dir = Directory.GetCurrentDirectory() + "Temp";

    public async Task Download(string url)
    {
        Console.WriteLine("Downloading");
        YouTubeVideo? video = null;
        using (var youTube = Client.For(new VideoLibrary.YouTube()))
        {
            video = await youTube.GetVideoAsync(url);
        }

        Console.WriteLine(video.Title);
        File.WriteAllBytes(_dir + video.FullName, video.GetBytes());
    }
}