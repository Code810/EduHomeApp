using EduHomeApp.Data;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();
            services.AddDbContext<EduHomeDbContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSession();
            services.AddHttpContextAccessor();

        }
    }
}
