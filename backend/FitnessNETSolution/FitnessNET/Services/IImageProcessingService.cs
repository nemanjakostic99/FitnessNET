using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FitnessNET.Services;

public interface IImageProcessingService
{
    Task<byte[]> ProcessProfileImageAsync(Stream imageStream);
} 