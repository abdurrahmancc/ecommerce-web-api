using Ecommerce_web_api.data;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Ecommerce_web_api.Configurations;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;


namespace Ecommerce_web_api.Services
{
    public class FilesUploadService : IFilesUploadService
    {

        private readonly AppDbContext _context;
        private readonly Cloudinary _cloudinary;
        public FilesUploadService(AppDbContext context, IOptions<CloudinarySettings> settings)
        {
            _context = context;

            // Extract credentials from Cloudinary URL
            var cloudinaryUrl = settings.Value.CloudinaryUrl;
            
            if (string.IsNullOrEmpty(cloudinaryUrl))
            {
                throw new ArgumentException("Cloudinary URL is not configured properly.");
            }

            var uri = new Uri(cloudinaryUrl);
            var credentials = uri.UserInfo.Split(':');
            if (credentials.Length != 2)
            {
                throw new ArgumentException("Cloudinary URL must contain valid API key and secret.");
            }

            var cloudName = uri.Host;
            var apiKey = credentials[0];
            var apiSecret = credentials[1];

            // Create the Account object
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
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
              var  appDomain =   _env.APIENDPOINT;
            // Generate the URL for the uploaded file
            string fileUrl = $"{appDomain}/uploads/{fileName}";
            fileUrls.Add(fileUrl);
        }
    }

    await _context.SaveChangesAsync();

    // Return a string if only one file is uploaded, or a list of strings if multiple files are uploaded
    return fileUrls.Count == 1 ? fileUrls[0] : fileUrls;
}


        public async Task<List<string>> CloudinaryUploadFileAsync(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("No files provided.");
            }

            var uploadedUrls = new List<string>();

            foreach (var file in files)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "uploads", // Optional: Configure upload folder
                    };



                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");
                    }


                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        uploadedUrls.Add(uploadResult.SecureUrl.ToString());
                    }
                    else
                    {
                        throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");
                    }
                }
            }

            return uploadedUrls;
        }
    }
}
