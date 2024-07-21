using System.Text.RegularExpressions;

namespace EduHomeApp.Extensions
{
    public static class Email
    {
        public static bool CheckEmail(string email)
        {
            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            return match.Success;
        }
    }
}
