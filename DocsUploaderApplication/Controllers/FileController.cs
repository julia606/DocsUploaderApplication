using Microsoft.AspNetCore.Mvc;

namespace DocsUploaderApplication.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
