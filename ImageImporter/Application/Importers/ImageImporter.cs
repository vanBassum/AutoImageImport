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

        public async Task<ImportResult> ImportFile(string source)
        {
            ImportResult result = new ImportResult();
            IImageInfo imageInfo = Image.Identify(source);

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
                    var hash = await hashGenerator.Generate(source);
                    try
                    {
                        result = ImportLogic(source, hash);
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


        ImportResult ImportLogic(string source, byte[]? hash)
        {
            ImportResult result = new ImportResult();

            var relPath = Path.GetRelativePath(Settings.ImageImportFolder, source);
            var destination = Path.Combine(Settings.ImageExportFolder, relPath);

            var match = Context.Pictures.FirstOrDefault(a => a.Hash == hash);
            if (match == null)
            {
                if (File.Exists(destination))
                {
                    destination = ImporterHelpers.RenameDuplicates(destination);
                    relPath = Path.GetRelativePath(Settings.ImageExportFolder, destination);
                    DoImport(source, destination, relPath, hash);
                    result.Status = ImportStatus.ImportedUniqueFileWithRename;
                    result.Success = true;
                }
                else
                {
                    DoImport(source, destination, relPath, hash);
                    result.Status = ImportStatus.ImportedUniqueFile;
                    result.Success = true;
                }
            }
            else
            {
                var matchFile = Path.Combine(Settings.ImageExportFolder, match.Path);
                if (ImporterHelpers.FileEquals(source, matchFile))
                {
                    File.Delete(source);
                    result.Status = ImportStatus.ExactDuplicateDeletedSource;
                    result.Success = true;
                }
                else
                {
                    result = ImportSameFileLogic(source, destination);
                }
            }
            return result;
        }



        ImportResult ImportSameFileLogic(string source, string destination)
        {
            ImportResult result = new ImportResult();



            if (CheckIfSourceIsBetter(source, destination))
            {
                //Keep source
            }



            return result;
        }

        void DeleteFile(string file, string relPath)
        {
            if (Settings.ImageRecycleMatches)
            {
                var recyleDestination = Path.Combine(Settings.ImageRecycleFolder, relPath);



                //if (File.Exists(destination))
                //{
                //    //destination = ImporterHelpers.RenameDuplicates(destination);
                //    //KeepBestQuality(source, destination);
                //    //result.Status = ImportStatus.MatchingDuplicateRecycledAndRenamedSource;
                //    //result.Success = true;
                //}
                //else
                //{
                //    if (CheckIfSourceIsBetter(source, destination))
                //    {
                //        //File.Delete(destination);
                //        //File.Move(source, destination);
                //        //result.Status = ImportStatus.MatchingDuplicateKeptSource;
                //        //result.Success = true;
                //    }
                //    else
                //    {
                //        //MoveFile(source, destination);
                //        //result.Status = ImportStatus.MatchingDuplicateRecycledSource;
                //        //result.Success = true;
                //    }
                //}
            }
        }


        bool CheckIfSourceIsBetter(string source, string destination)
        {
            var infoSource = new FileInfo(source);
            var infoDestination = new FileInfo(destination);
            return infoSource.Length > infoDestination.Length;
        }


        void MoveFile(string source, string destination)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            File.Move(source, destination);
        }


        void DoImport(string source, string destination, string relPath, byte[]? hash)
        {
            MoveFile(source, destination);
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

                file = Path.Combine(path, fileName + extention);
            }
            return file;
        }

        //https://stackoverflow.com/questions/968935/compare-binary-files-in-c-sharp
        public static bool FileEquals(string fileName1, string fileName2)
        {
            // Check the file size and CRC equality here.. if they are equal...    
            using (var file1 = new FileStream(fileName1, FileMode.Open))
            using (var file2 = new FileStream(fileName2, FileMode.Open))
                return FileStreamEquals(file1, file2);
        }

        static bool FileStreamEquals(Stream stream1, Stream stream2)
        {
            const int bufferSize = 2048;
            byte[] buffer1 = new byte[bufferSize]; //buffer size
            byte[] buffer2 = new byte[bufferSize];
            while (true)
            {
                int count1 = stream1.Read(buffer1, 0, bufferSize);
                int count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                    return true;

                if (count1 == 0 && count2 == 0)
                    return true;

                if (!buffer1.SequenceEqual(buffer2))
                    return false;


            }
        }

    }


}
