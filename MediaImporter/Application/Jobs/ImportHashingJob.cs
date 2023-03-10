using MediaImporter.Application.Services;
using MediaImporter.Models;
using Mica;
using Microsoft.EntityFrameworkCore;

namespace MediaImporter.Application.Jobs
{
    [JobConcurrency(true)]
    public class ImportHashingJob : IJob
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;
        public ImportHashingJob(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        async Task<ImportFile?> FindAndClaimFile()
        {
            var importFile = await _context.ImportFiles.FirstOrDefaultAsync(f => f.Claim < DateTime.Now && string.IsNullOrEmpty(f.Hash));
            if (importFile != null)
            {
                DateTime claim = DateTime.Now + TimeSpan.FromSeconds(60); //Claim this file for 60 seconds.
                importFile.Claim = claim;
                await _context.SaveChangesAsync();
                await _context.Entry(importFile).ReloadAsync();
                if (claim == importFile.Claim)
                    return importFile;
            }
            return null;
        }


        public async Task Work(IProgress<double> progress, CancellationToken token = default)
        {
            var importFile = await FindAndClaimFile();
            if (importFile != null)
            {
                importFile.Hash = await _fileService.CalculateHash(importFile.Path);
                importFile.Claim = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
