using Ecommerce_web_api.DTOs.User;
using Ecommerce_web_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ecommerce_web_api.Controllers
{
    [Route("v1/api/filesUpload/")]
    [ApiController]
    public class FilesUploadController : ControllerBase
    {
        private readonly IFilesUploadService _filesUploadService;

        public FilesUploadController(IFilesUploadService filesUploadService)
        {
            _filesUploadService = filesUploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were uploaded.");
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            try
            {
                var result = await _filesUploadService.SaveFilesAsync(files, uploadPath);

                if (result is string singleFileUrl)
                {
                    return Ok(ApiResponse<string>.SuccessResponse(singleFileUrl, 200, "File uploaded successfully."));
                }

                if (result is List<string> multipleFileUrls)
                {
                    return Ok(ApiResponse<List<string>>.SuccessResponse(multipleFileUrls, 200, "Files uploaded successfully."));
                }

                return BadRequest("An unexpected error occurred.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while uploading files.", Error = ex.Message });
            }
        }



        [HttpPost("upload-cloudinary")]
        public async Task<IActionResult> UploadFilesToCloudinary([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were uploaded.");
            }

            try
            {
                var urls = await _filesUploadService.CloudinaryUploadFileAsync(files);
                return Ok(ApiResponse<List<string>>.SuccessResponse(urls, 200, "Files uploaded to Cloudinary successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(new List<string> { ex.Message }, 404, "Cloudinary upload failed."));
            }
        }
    }
}
