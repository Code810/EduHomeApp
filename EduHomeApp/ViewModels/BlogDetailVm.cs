namespace EduHomeApp.ViewModels
{
    public class BlogDetailVm
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public string AppUser { get; set; }
        public DateTime CreateDate { get; set; }
        public ICollection<CategoryListVm> Categories { get; set; }
    }
}
