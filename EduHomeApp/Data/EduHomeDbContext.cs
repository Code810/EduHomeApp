using EduHomeApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Data
{
    public class EduHomeDbContext : IdentityDbContext<AppUser>
    {
        public EduHomeDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<HomeContent> HomeContents { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CourseLanguage> CourseLanguages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CourseTag> CourseTags { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<EventSpeaker> EventSpeakers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<AboutArea> AboutArea { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherContact> TeacherContacts { get; set; }
        public DbSet<Student> Students { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
