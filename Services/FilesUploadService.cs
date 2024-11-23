using Ecommerce_web_api.data;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;

namespace Ecommerce_web_api.Services
{
    public class FilesUploadService : IFilesUploadService
    {

        private readonly AppDbContext _context;

        public FilesUploadService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<object?> SaveFilesAsync(List<IFormFile> files, string uploadPath)
{
    if (files == null || !files.Any())
    {
        throw new ArgumentException("No files provided.");
    }

    var fileUrls = new List<string>();

    foreach (var file in files)
    {
        if (file.Length > 0)
        {
            // Generate a new GUID for each file
            string guid = Guid.NewGuid().ToString();

            // Extract the original file name without the extension
            string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);

            // Extract the file extension (e.g., .png, .jpg)
            string fileExtension = Path.GetExtension(file.FileName);

            // Combine the GUID, original file name, and file extension
            string fileName = $"{guid}_{originalFileName}{fileExtension}";

            // Combine the upload path and the new file name to create the full file path
            string filePath = Path.Combine(uploadPath, fileName);

            // Ensure directory exists
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Save file details to the database
            var fileModel = new FilesUploadModel
            {
                FileName = fileName,
                FilePath = filePath,
            };

            _context.FilesUploads.Add(fileModel);

            // Generate the URL for the uploaded file
            string fileUrl = $"https://yourdomain.com/uploads/{fileName}";
            fileUrls.Add(fileUrl);
        }
    }

    await _context.SaveChangesAsync();

    // Return a string if only one file is uploaded, or a list of strings if multiple files are uploaded
    return fileUrls.Count == 1 ? fileUrls[0] : fileUrls;
}



    }
}
