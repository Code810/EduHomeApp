namespace EduHomeApp.Helpers
{
    public class Helper
    {
        public static void DeleteImage(string folderName, string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folderName, fileName);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
    }
}
