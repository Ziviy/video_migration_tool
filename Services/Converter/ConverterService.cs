using encouraging_bot.Hostings.YouTube;
using FFMpegCore;
using FFMpegCore.Enums;

namespace encouraging_bot.Services.Converter;

public class ConverterService : IConverterService
{
    private readonly IVideoProbeService _prober;

    public ConverterService(
        IVideoProbeService prober)
    {
        _prober = prober;
    }

    public async Task ConvertAsync(VideoObj video, CancellationToken cancellationToken)
    {
        // var mediaInfo = await _prober.Probe(_video.Dir + _video.TmpVideoName);

        var values = Enum.GetValues(typeof(AudioQuality));
        var quality = (AudioQuality)128;
        
        var tmp1 = video.Dir + video.TmpVideoName;
        var tmp2 = video.Dir + video.TmpAudioName;
        Console.WriteLine();
        await FFMpegArguments
            .FromFileInput(video.Dir + video.TmpVideoName)
            .AddFileInput(video.Dir + video.TmpAudioName)
        // OutputToFile(output, addArguments: (Action<FFMpegArgumentOptions>) (options => options.CopyChannel().WithAudioCodec(AudioCodec.Aac).WithAudioBitrate(AudioQuality.Good).UsingShortest(stopAtShortest))).ProcessSynchronously()
            .OutputToFile(video.Dir + "2-" + video.TmpVideoName, true, addArguments: (Action<FFMpegArgumentOptions>) (options => options
                .WithVideoCodec("vp9")
                .ForceFormat("mkv")
                .CopyChannel()
                .WithAudioCodec(AudioCodec.Aac)
                .WithAudioBitrate((AudioQuality)128)
                .UsingShortest(false)))
            .ProcessAsynchronously();
        
        FFMpeg.ReplaceAudio(video.Dir + "2-" + video.TmpVideoName, video.Dir + video.TmpAudioName, video.Dir + video.VideoName);
    }
}