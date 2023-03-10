using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace MediaImporter.Models
{
    [Index(nameof(Path), IsUnique = true)]
    public class ImportFile
    {
        [Key]
        public int Id { get; set; }

        public virtual WebFile? File { get; set; }

        public string? Hash { get; set; }

        [ConcurrencyCheck]
        public DateTime Claim { get; set; }

    }

}
