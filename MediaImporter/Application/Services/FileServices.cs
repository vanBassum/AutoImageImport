using MediaImporter.Models;
using Mica;
using Microsoft.Extensions.Options;
using System.IO;

namespace MediaImporter.Application.Services
{
    public class FileService
    {
        private readonly Settings _settings; 
        public FileService(IOptions<Settings> settings)
        {
            _settings = settings.Value;

            //Ensure folders exist
            Directory.CreateDirectory(_settings.ImportFolder);

        }

        public IEnumerable<WebFile> GetImportFiles()
        {
            var files = Directory.GetFiles(_settings.ImportFolder, "*.*", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                WebFile webFile = new WebFile();
                webFile.Path = Path.GetRelativePath(_settings.ImportFolder, file);
                yield return webFile;
            }
        }


        public async Task<string> CalculateHash(WebFile file)
        {

        }

    }
}
