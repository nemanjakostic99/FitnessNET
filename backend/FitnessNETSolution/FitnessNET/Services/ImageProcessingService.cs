using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace FitnessNET.Services;

public class ImageProcessingService : IImageProcessingService
{
    private const int TargetSize = 800;
    private readonly ILogger<ImageProcessingService> _logger;

    public ImageProcessingService(ILogger<ImageProcessingService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> ProcessProfileImageAsync(Stream imageStream)
    {
        try
        {
            using var image = await Image.LoadAsync(imageStream);
            
            // Calculate dimensions to maintain aspect ratio while fitting in 800x800
            var rectangle = GetTargetRectangle(image.Width, image.Height);

            image.Mutate(x => x
                .Crop(rectangle)
                .Resize(TargetSize, TargetSize));

            using var outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream, new JpegEncoder
            {
                Quality = 85 // Good balance between quality and file size
            });

            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing profile image");
            throw new ApplicationException("Failed to process the profile image", ex);
        }
    }

    private static Rectangle GetTargetRectangle(int width, int height)
    {
        int size = Math.Min(width, height);
        int x = (width - size) / 2;
        int y = (height - size) / 2;

        return new Rectangle(x, y, size, size);
    }
} 