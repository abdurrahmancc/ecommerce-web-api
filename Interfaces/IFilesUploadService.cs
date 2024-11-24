namespace Ecommerce_web_api.Interfaces
{
    public interface IFilesUploadService
    {
        Task<object?>  SaveFilesAsync(List<IFormFile> files, string uploadPath);
        Task<List<string>> CloudinaryUploadFileAsync(List<IFormFile> files);
    }
}
