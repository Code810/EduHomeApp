
using EduHomeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduHomeApp.Data.Configurations
{
    public class CourseLanguageConfiguration : IEntityTypeConfiguration<CourseLanguage>
    {
        public void Configure(EntityTypeBuilder<CourseLanguage> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}
