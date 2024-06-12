using Microsoft.AspNetCore.Http;

namespace FootballHub.Application.Interfaces;

public interface IFileConverter
{
    IFormFile ReadFile(IFormFile file);
    byte[] ConvertToByteArray(IFormFile file);
}
