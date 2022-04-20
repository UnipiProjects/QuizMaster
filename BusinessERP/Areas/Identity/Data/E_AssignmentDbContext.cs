using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessERP.Areas.Identity.Data;
using BusinessERP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusinessERP.Data
{
    public class BusinessERPDbContext : IdentityDbContext<ApplicationUser>
    {
        public BusinessERPDbContext(DbContextOptions<BusinessERPDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<Diploma> Diplomas { get; set; }
    }
}
