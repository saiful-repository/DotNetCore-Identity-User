using Microsoft.EntityFrameworkCore;
using DotNetCoreIdentity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DotNetCoreIdentity.Models;

namespace DotNetCoreIdentity.DBContexts
{
    public class ProductContext: IdentityDbContext<ApplicationUser>
    {
        public ProductContext(DbContextOptions<ProductContext> options):base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
