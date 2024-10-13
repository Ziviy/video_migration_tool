using FFMpegCore;

namespace encouraging_bot.Services.Converter;

public interface IVideoProbeService
{
    public Task<IMediaAnalysis> Probe(string filePath);
}