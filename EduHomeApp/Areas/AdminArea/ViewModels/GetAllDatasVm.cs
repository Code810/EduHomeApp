using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class GetAllDatasVm
    {
        public SelectList Categories { get; set; }
        public SelectList Teachers { get; set; }
        public SelectList CourseLanguages { get; set; }
        public SelectList Tags { get; set; }
    }
}
