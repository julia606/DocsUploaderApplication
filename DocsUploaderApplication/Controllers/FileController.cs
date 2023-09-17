using Azure.Storage.Blobs;
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
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            var email = Request.Form["email"].ToString();

            if (FileValidationService.IsFileSelected(file))
            {
                if (FileValidationService.IsFileValid(file, 10485760) && EmailValidationService.IsEmailValid(email))
                {
                    var containerClient = _blobServiceClient.GetBlobContainerClient("blobcontainer");
                    var blobClient = containerClient.GetBlobClient(file.FileName);

                    using (var stream = file.OpenReadStream())
                    {
                        blobClient.SetMetadata(new Dictionary<string, string>
                        {
                            { "Email", email }
                        });
                        await blobClient.UploadAsync(stream, true);
                    }

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
