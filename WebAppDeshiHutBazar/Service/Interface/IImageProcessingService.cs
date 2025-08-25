namespace WebDeshiHutBazar
{
    public interface IImageProcessingService
    {
        byte[] GetResizedImage(byte[] imageData, int maxWidth, int maxHeight);

        byte[] Resize_Picture(byte[] imageData, int FinalWidth, int FinalHeight, int ImageQuality);
    }
}

