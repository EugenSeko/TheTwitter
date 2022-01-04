using Android.Graphics;
using Android.Media;
using InterTwitter.Services.VideoProcessing;
using System;
using System.IO;
using Stream = System.IO.Stream;

namespace InterTwitter.Droid.Services.VideoProcessing
{
    public class VideoProcessingService : IVideoProcessingService
    {
        public double TryGetVideoLength(string url)
        {
            double result;

            try
            {
                MediaMetadataRetriever retriever = new MediaMetadataRetriever();
                retriever.SetDataSource(url);

                var length = retriever.ExtractMetadata(MetadataKey.Duration);
                result = Convert.ToDouble(length) / 1000;
            }
            catch(Exception e)
            {
                result = -1;
            }

            return result;
        }

        public Stream TryGetFrameAtTime(string url, long usecond)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                MediaMetadataRetriever retriever = new MediaMetadataRetriever();
                retriever.SetDataSource(url);

                Bitmap bitmap = retriever.GetFrameAtTime(usecond);
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
            }
            catch (Exception e)
            {
                stream = null;
            }

            return stream;
        }
    }
}