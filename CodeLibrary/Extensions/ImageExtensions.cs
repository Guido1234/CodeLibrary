using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CodeLibrary.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ConvertImageToByteArray(this Image imageToConvert)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, ImageFormat.Jpeg);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        public static byte[] ConvertImageToByteArray(this Image imageToConvert, long quality)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, quality);
            ImageCodecInfo.GetImageEncoders();

            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, jgpEncoder, myEncoderParameters);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}