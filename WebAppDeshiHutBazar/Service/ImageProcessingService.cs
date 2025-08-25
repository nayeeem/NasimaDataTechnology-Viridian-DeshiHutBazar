using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace WebDeshiHutBazar
{
    public class ImageProcessingService : IImageProcessingService
    {
        public byte[] GetResizedImage(byte[] imageData, int maxWidth, int maxHeight)
        {
            Bitmap image = GetBitMapFromByteAray(imageData);
            image = GetResizedImage(image, maxWidth, maxHeight);
            return GetCompressedImage(image);
        }

        private byte[] GetCompressedImage(Bitmap image)
        {
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);//100L for least compresion //0L for greatest compression
            myEncoderParameters.Param[0] = myEncoderParameter;

            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, jpgEncoder, myEncoderParameters);

            return memoryStream.ToArray();
        }

        private Bitmap GetResizedImage(Bitmap image, int maxWidth, int maxHeight)
        {
            // Get the image's original width and height
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
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

        private Bitmap GetBitMapFromByteAray(byte[] imageData)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(imageData))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }

        public byte[] Resize_Picture(byte[] imageData, int FinalWidth, int FinalHeight, int ImageQuality)
        {
            System.Drawing.Bitmap NewBMP;
            System.Drawing.Graphics graphicTemp;
            System.Drawing.Bitmap bmp = GetBitMapFromByteAray(imageData);
            int originalWidth = bmp.Width;
            int originalHeight = bmp.Height;
            if(originalWidth > originalHeight)
            {
                FinalHeight = 0;
            }
            else
            {
                FinalWidth = 0;
            }
            int iWidth;
            int iHeight;
            if ((FinalHeight == 0) && (FinalWidth != 0))
            {
                iWidth = FinalWidth;
                iHeight = (bmp.Size.Height * iWidth / bmp.Size.Width);
            }
            else if ((FinalHeight != 0) && (FinalWidth == 0))
            {
                iHeight = FinalHeight;
                iWidth = (bmp.Size.Width * iHeight / bmp.Size.Height);
            }
            else
            {
                iWidth = FinalWidth;
                iHeight = FinalHeight;
            }

            NewBMP = new System.Drawing.Bitmap(iWidth, iHeight);
            graphicTemp = System.Drawing.Graphics.FromImage(NewBMP);
            graphicTemp.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            //graphicTemp.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            //graphicTemp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            //graphicTemp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //graphicTemp.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
            graphicTemp.DrawImage(bmp, 0, 0, iWidth, iHeight);
            graphicTemp.Dispose();
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 8);
            encoderParams.Param[0] = encoderParam;
            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            MemoryStream memoryStream = new MemoryStream();
            for (int fwd = 0; fwd <= arrayICI.Length - 1; fwd++)
            {
                if (arrayICI[fwd].FormatDescription.Equals("JPEG"))
                {
                    NewBMP.Save(memoryStream, arrayICI[fwd], encoderParams);
                }
            }
            NewBMP.Dispose();
            bmp.Dispose();
            return memoryStream.ToArray();            
        }
    }
}
