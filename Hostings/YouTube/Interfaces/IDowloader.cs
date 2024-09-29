namespace encouraging_bot.YouTube.Interfaces;
using VideoLibrary;

public interface IDownloader
{
        Task Download(string url);
}