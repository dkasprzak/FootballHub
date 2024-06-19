using Microsoft.AspNetCore.Http;

namespace FootballHub.Application.Helpers;

public static class FileConverter
{
    public static string ConvertFileToBase64String(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        
        var fileData = memoryStream.ToArray();
        var fileContentType = file.ContentType;
        var fileDataString = Convert.ToBase64String(fileData);
        
        var base64 = $"data:{fileContentType};base64,{fileDataString}";
        return base64;
    }
}
