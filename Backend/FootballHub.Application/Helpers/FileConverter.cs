using Microsoft.AspNetCore.Http;

namespace FootballHub.Application.Services;

public static class FileConverter
{
    public static IFormFile ReadFile(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return file;
    }

    public static byte[] ConvertToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();    
    }
}
