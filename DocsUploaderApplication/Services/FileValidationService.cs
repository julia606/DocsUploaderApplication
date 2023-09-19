namespace DocsUploaderApplication.Services
{
    public class FileValidationService
    {
        public static bool IsFileValid(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);

            if (!string.Equals(fileExtension, ".docx", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static bool IsFileSelected(IFormFile file)
        {
            if (file.Length > 0 && file != null)
            {
                return true;
            }
            return false;
        }
    }
}
