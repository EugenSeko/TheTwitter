using InterTwitter.Enums;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services.VideoProcessing;
using System;
using System.IO;
using System.Threading.Tasks;
using VideoTrimmer.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterTwitter.Services.Video
{
    public class VideoService : IVideoService
    {
        private IVideoProcessingService _videoProcessingService;

        public VideoService(IVideoProcessingService videoProcessingService)
        {
            _videoProcessingService = videoProcessingService;
        }

        public async Task<AOResult<VideoProcessingResultModel>> ProcessingVideoForPostAsync(string sourceFilePath)
        {
            EVideoProcessingResult message;
            var result = new AOResult<VideoProcessingResultModel>();

            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + (Device.RuntimePlatform == Device.iOS ? ".MOV" : ".mp4");
                var videoFilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                string fileNameThumb = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                var frameFilePath = Path.Combine(FileSystem.AppDataDirectory, fileNameThumb);

                FileInfo fileInf = new FileInfo(sourceFilePath);

                if (fileInf.Exists)
                {
                    if (fileInf.Length <= Constants.Limits.MAX_SIZE_ATTACHED_VIDEO)
                    {
                        var videoLength = _videoProcessingService.TryGetVideoLength(sourceFilePath);

                        if (videoLength > Constants.Limits.MAX_LENGTH_VIDEO)
                        {
                            if (await VideoTrimmerService.Instance.TrimAsync(0, Constants.Limits.MAX_LENGTH_VIDEO * 1000, sourceFilePath, videoFilePath))
                            {
                                message = EVideoProcessingResult.Success;
                            }
                            else
                            {
                                message = EVideoProcessingResult.Error;
                            }
                        }
                        else
                        {
                            File.Copy(sourceFilePath, videoFilePath);
                            message = EVideoProcessingResult.Success;
                        }

                        if (message == EVideoProcessingResult.Success)
                        {
                            if (_videoProcessingService.TryGetFrameAtTime(sourceFilePath, (long)(videoLength / 2)) is Stream image)
                            {
                                image.CopyTo(new FileStream(frameFilePath, System.IO.FileMode.OpenOrCreate));
                            }
                            else
                            {
                                message = EVideoProcessingResult.Error;
                            }
                        }
                    }
                    else
                    {
                        message = EVideoProcessingResult.LimitSizeVideo;
                    }
                }
                else
                {
                    message = EVideoProcessingResult.FileNotExist;
                }

                if (message == EVideoProcessingResult.Success)
                {
                    result.SetSuccess(new VideoProcessingResultModel()
                    {
                        FrameFilePath = frameFilePath,
                        VideoFilePath = videoFilePath,
                        Message = message,
                    });
                }
                else
                {
                    result.SetFailure(new VideoProcessingResultModel()
                    {
                        Message = message,
                    });
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(ProcessingVideoForPostAsync)}: exception", "Some issues", ex);
            }

            return result;
        }
    }
}
