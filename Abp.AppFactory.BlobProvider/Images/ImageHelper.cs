using ImageSharp;
using ImageSharp.Colors.Spaces;
using ImageSharp.Formats;

namespace Abp.AppFactory.BlobProvider.Images
{
    public static class ImageHelper
    {
        public static readonly IImageFormat[] DefaultFormats = { new BmpFormat(), new JpegFormat(), new PngFormat() };

        public static IImageFormat GetFormat(byte[] data)
        {
            var image = new Image<ImageSharp.Rgba64>(data);
            return getFormat(image);
        }

        public static bool HasFormat(byte[] data, params IImageFormat[] formats)
        {
            var imageFormat = GetFormat(data);
            if (formats == null || formats.Length == 0)
            {
                formats = DefaultFormats;
            }

            foreach (var format in formats)
            {
                if (imageFormat.Extension == format.Extension &&
                    imageFormat.MimeType == format.MimeType &&
                    imageFormat.HeaderSize == imageFormat.HeaderSize)
                {
                    return true;
                }
            }
            return false;
        }

        private static IImageFormat getFormat(Image<Rgba64> image)
        {
            return image.CurrentImageFormat;
        }
    }
}