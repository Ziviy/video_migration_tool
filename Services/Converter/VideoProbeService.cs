using FFMpegCore;

namespace encouraging_bot.Services.Converter;

public class VideoProbeService : IVideoProbeService
{
    public async Task<IMediaAnalysis> Probe(string filePath)
    {
        var mediaInfo = await FFProbe.AnalyseAsync(filePath);
        return mediaInfo;
    }
}