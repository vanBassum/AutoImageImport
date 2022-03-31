// See https://aka.ms/new-console-template for more information
using ImageImporter.Models;
using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public DbSet<Picture> Pictures { get; set; }

    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseMySQL("Server=h2954374.stratoserver.net;Database=ImageImporter;Uid=ImageImporter;Pwd=v96RBJx9mat2C4V4;");
        
        optionsBuilder.UseLazyLoadingProxies();
    }

}


