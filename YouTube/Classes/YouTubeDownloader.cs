using encouraging_bot.YouTube.Interfaces;
using VideoLibrary;
namespace encouraging_bot.YouTube.Classes;

public class YouTubeDownloader : IDownloader
{
    public void Download(string url, string path)
    {
        var youTube = VideoLibrary.YouTube.Default;
        YouTubeVideo video = youTube.GetVideo(url);
        // File.WriteAllBytes();
    }
}