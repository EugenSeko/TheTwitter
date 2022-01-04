using System.IO;

namespace InterTwitter.Services.VideoProcessing
{
    public interface IVideoProcessingService
    {
        double TryGetVideoLength(string url);

        Stream TryGetFrameAtTime(string url, long usecond);
    }
}