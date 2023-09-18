using Azure.Storage.Blobs;
using DocsUploaderApplication.Models;
using DocsUploaderApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocsUploaderApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly BlobServiceClient _blobServiceClient;
        public FileController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadModel model)
        {
            var file = model.File;
            var email = model.Email;

            if (FileValidationService.IsFileSelected(file))
            {
                if (FileValidationService.IsFileValid(file, 10485760) && EmailValidationService.IsEmailValid(email))
                {
                    var containerClient = _blobServiceClient.GetBlobContainerClient("blobcontainer");
                    var blobClient = containerClient.GetBlobClient(file.FileName);

                    using (var stream = file.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }
                    await blobClient.SetMetadataAsync(new Dictionary<string, string>
                         {
                             { "Email", email }
                         });

                    return Ok($"File uploaded successfully.");
                }
                return BadRequest("Invalid data");
            }
            else
            {
                return BadRequest("No file selected.");
            }
        }
    }
}
