using VideoLibrary;

namespace encouraging_bot.Hostings.YouTube;

public class VideoObj
{
    public string TmpVideoName { get; }
    public string TmpAudioName { get; }
    public string Dir { get; }
    public string VideoName { get; }
    public YouTubeVideo AudioCodec { get; }

    public VideoObj(string tmpVideoName, string tmpAudioName, string dir, string videoName, YouTubeVideo audioCodec)
    {
        this.TmpVideoName = tmpVideoName;
        this.TmpAudioName = tmpAudioName;
        this.Dir = dir;
        this.VideoName = videoName;
        this.AudioCodec = audioCodec;
    }
}