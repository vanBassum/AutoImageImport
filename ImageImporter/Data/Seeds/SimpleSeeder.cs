using Microsoft.EntityFrameworkCore;

using ImageImporter.Data;

namespace DB4045.Data.Seeds
{
    /// <summary>
    /// Only adds Seeds to database if the table is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SimpleSeeder<T> : ISeeder where T : class
    {
        ApplicationDbContext Database { get; }

        public SimpleSeeder(ApplicationDbContext db)
        {
            Database = db;
        }

        protected abstract T[] Seeds { get; }


        public async Task Execute()
        {
            if(!Database.Set<T>().Any())
            {
                foreach (var obj in Seeds)
                    Database.Add(obj);
                await Database.SaveChangesAsync();
            }
        }
    }
}
