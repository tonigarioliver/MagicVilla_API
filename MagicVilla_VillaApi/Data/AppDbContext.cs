using MagicVilla_VillaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MagicVilla_VillaApi.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Villa>Villas { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
    }
}
