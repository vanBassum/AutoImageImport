using MediaImporter.Application.Services;
using MediaImporter.Models;
using Mica;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MediaImporter.Application.Jobs
{
    [JobConcurrency(false)]
    public class ImportScanJob : IJob
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;
        public ImportScanJob(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task Work(IProgress<double> progress, CancellationToken token = default)
        {
            var files = Directory.GetFiles(_fileService.ImportFolder, "*.*", SearchOption.AllDirectories);
            for(int i=0; i<files.Length; i++)
            {
                var file = files[i];
                var relativePath = _fileService.GetRelativePath(file);
                var webFile = await _context.ImportFiles.FirstOrDefaultAsync(a => a.Path == relativePath);

                if (webFile == null)
                {
                    ImportFile importFile = new ImportFile();
                    importFile.Path = relativePath;
                    _context.Add(importFile);
                    await _context.SaveChangesAsync();
                }

                token.ThrowIfCancellationRequested();
                progress.Report((double)i / files.Length);
            }
        }
    }
}
