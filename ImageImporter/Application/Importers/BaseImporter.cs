using ImageImporter.Models.Db;
using ImageImporter.Models.Enums;
using System.Text.RegularExpressions;

namespace ImageImporter.Application.Importers
{
    public abstract class BaseImporter : IImporter
    {
        protected abstract string ImportFolder { get; }
        protected abstract string ExportFolder { get; }
        protected abstract string RecylceFolder { get; }
        protected abstract bool UseRecycleFolder { get; }
        protected abstract Task<bool> CheckFile(string source);
        protected abstract Task<byte[]?> CalculateHash(string source);
        protected abstract Task<string?> FindExistingByHash(byte[] hash);
        protected abstract Task<bool?> CheckIfSourceIsBetter(string source, string destination);
        protected virtual async Task AfterImport(ImportResult import) { return; }



        public async Task ImportFile(string source, ImportResult result)
        {
            try
            {
                string relPath = Path.GetRelativePath(ImportFolder, source);
                result.RelativePath = relPath;

                if (!await CheckFile(source))
                {
                    result.SetResult(false, ImportStatus.IncorrectFileType);
                    return;
                }

                var hash = await CalculateHash(source);
                if(hash == null)
                {
                    result.SetResult(false, ImportStatus.HashingFailed);
                    return;
                }
                result.Hash = hash;

                string destination = Path.Combine(ExportFolder, relPath);
                string? existing = await FindExistingByHash(hash);

                if (existing == null)
                {
                    destination = RenameUntillUnique(destination);
                    Directory.CreateDirectory(Path.GetDirectoryName(destination));
                    File.Move(source, destination);
                    result.SetResult(true, ImportStatus.ImportedUniqueFile);
                    await AfterImport(result);
                }
                else
                {
                    existing = Path.Combine(ExportFolder, existing);
                    var sourceIsBetter = await CheckIfSourceIsBetter(source, existing);
                    if (!sourceIsBetter.HasValue)
                    {
                        result.SetResult(false, ImportStatus.CoulntDetermineBestQuality);
                    }
                    else
                    {
                        if (sourceIsBetter.Value)
                        {
                            DeleteFile(existing);
                            MoveFile(source, existing);
                            result.SetResult(true, ImportStatus.HashMatchKeptSource);
                            await AfterImport(result);
                        }
                        else
                        {
                            DeleteFile(source);
                            result.SetResult(false, ImportStatus.HashMatchDeletedSource);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                result.SetException(false, exception);
            }
        }


        public void MoveFile(string source, string destination)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            File.Move(source, destination);
        }

        public void DeleteFile(string file)
        {
            if(UseRecycleFolder)
            {
                string relPath = Path.GetRelativePath(ImportFolder, file);
                string destination = Path.Combine(RecylceFolder, relPath);
                destination = RenameUntillUnique(destination);
                MoveFile(file, destination);
            }
            else
            {
                File.Delete(file);
            }
        }


        public string RenameUntillUnique(string file)
        {
            string path = Path.GetDirectoryName(file);
            string fileName = Path.GetFileNameWithoutExtension(file);
            string extention = Path.GetExtension(file);
            while (File.Exists(file))
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
    }


}
