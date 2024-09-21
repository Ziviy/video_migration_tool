namespace encouraging_bot.YouTube.Interfaces;
using VideoLibrary;

public interface IDownloader
{
    void Download(string url, string path);
}