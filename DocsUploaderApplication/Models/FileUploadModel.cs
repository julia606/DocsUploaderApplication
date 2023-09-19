using System.ComponentModel.DataAnnotations;

namespace DocsUploaderApplication.Models
{
    public class FileUploadModel
    {
        [Required]
        public IFormFile? File { get; set; }
        
        [Required]
        public string? Email { get; set; }
    }
}
