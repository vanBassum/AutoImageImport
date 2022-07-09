using ImageImporter.Application.Hashing;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Services;
using SixLabors.ImageSharp;
using System.Text.RegularExpressions;

namespace ImageImporter.Application.Importers
{
    public class ImageImporter : IImporter
    {
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        public ImageImporter(Settings settings, ApplicationDbContext context)
        {
            Settings = settings;
            Context = context;
        }

        public async Task<ImportResult> ImportFile(FileInfo file)
        {
            ImportResult result = new ImportResult();
            IImageInfo imageInfo = Image.Identify(file.FullName);

            if (imageInfo != null)
            {
                IImageHashGenerator? hashGenerator = null;

                switch(Settings.ImageHashingAlgorithm)
                {
                    case ImageHashingAlgorithms.AHashing:
                        hashGenerator = new ImageAHashGenerator();
                        break;
                    case ImageHashingAlgorithms.DHashing:
                        hashGenerator= new ImageDHashGenerator();
                        break;   
                }
                
                if(hashGenerator!=null)
                {
                    hashGenerator.HashWidth = Settings.ImageHashWidth;
                    hashGenerator.HashHeight = Settings.ImageHashHeight;
                    var hash = await hashGenerator.Generate(file.FullName);
                    try
                    {
                        result = ImportLogic(file, hash);
                    }
                    catch (Exception ex)
                    {
                        result.Exception = ex;
                        result.Status = ImportStatus.ExceptionThrown;
                        result.Success = false;
                    }
                }
                else
                {
                    result.Status = ImportStatus.HashingAlgorithmUndetermined;
                    result.Success = false;
                }
            }
            else
            {
                result.Status = ImportStatus.IncorrectFileType;
                result.Success = false;
            }
            return result;
        }


        ImportResult ImportLogic(FileInfo file, byte[]? hash)
        {
            ImportResult result = new ImportResult();

            var match = Context.Pictures.FirstOrDefault(a => a.Hash == hash);
            if (match == null)
            {
                var relPath = Path.GetRelativePath(Settings.ImageImportFolder, file.FullName);
                var destination = Path.Combine(Settings.ImageExportFolder, relPath);

                if (File.Exists(destination))
                {
                    destination = ImporterHelpers.RenameDuplicates(destination);
                    relPath = Path.GetRelativePath(Settings.ImageExportFolder, destination);
                    DoImport(file.FullName, destination, relPath, hash);
                    result.Status = ImportStatus.ImportedUniqueFileWithRename;
                    result.Success = true;
                }
                else
                {
                    DoImport(file.FullName, destination, relPath, hash);
                    result.Status = ImportStatus.ImportedUniqueFile;
                    result.Success = true;
                }
            }
            else
            {
                //TODO full image comparison, keep best quality, remove duplicates (setting?)
            }

            return result;
        }



        void DoImport(string source, string destination, string relPath, byte[]? hash)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            File.Move(source, destination);
            Picture picture = new Picture();
            picture.Path = relPath;
            picture.Hash = hash;
            Context.Add(picture);
            Context.SaveChanges();
        }

    }


    public static class ImporterHelpers
    { 
    
        public static string RenameDuplicates(string file)
        {
            string path = Path.GetDirectoryName(file);
            string fileName = Path.GetFileNameWithoutExtension(file);
            string extention = Path.GetExtension(file);
            while(File.Exists(file))
            {
                var match = Regex.Match(fileName, @"(.+)\((\d+)\)$");
                if (match.Success)
                {
                    var number = int.Parse(match.Groups[2].Value);
                    number++;
                    fileName = $"{match.Groups[1].Value}({number.ToString()})";
                }
                else
                    fileName += "(1)";

                file = Path.Combine(path, file + extention);
            }
            return file;
        }
    
    }


}
