using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace DocsUploaderApplication.Controllers
{
    public class FileController : Controller
    {
        private readonly BlobServiceClient _blobServiceClient;
        public IActionResult Index()
        {
            return View();
        }
    }
}
