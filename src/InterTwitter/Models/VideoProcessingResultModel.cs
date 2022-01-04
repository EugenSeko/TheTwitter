using InterTwitter.Enums;

namespace InterTwitter.Models
{
    public class VideoProcessingResultModel
    {
        public string FrameFilePath { get; set; }

        public string VideoFilePath { get; set; }

        public EVideoProcessingResult Message { get; set; }
    }
}
