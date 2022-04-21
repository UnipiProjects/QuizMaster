using System;
using QuizMaster.Areas.Identity.Data;
using QuizMaster.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(QuizMaster.Areas.Identity.IdentityHostingStartup))]
namespace QuizMaster.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<QuizMasterDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("QuizMasterDbContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;
                })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<QuizMasterDbContext>();                    
            });
        }
    }
}