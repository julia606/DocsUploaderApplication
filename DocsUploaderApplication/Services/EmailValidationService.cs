using System.Text.RegularExpressions;

namespace DocsUploaderApplication.Services
{
    public class EmailValidationService
    {
        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            try
            {
                var res = Regex.IsMatch(email, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                return res;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
