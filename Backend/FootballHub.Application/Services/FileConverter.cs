using FootballHub.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FootballHub.Application.Services;

public class FileConverter : IFileConverter
{
    public IFormFile ReadFile(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return file;
    }

    public byte[] ConvertToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();    
    }
}
