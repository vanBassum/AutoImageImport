using MediaImporter.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaImporter.Application
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ImportFile> ImportFiles { get; set; }
    }
}
