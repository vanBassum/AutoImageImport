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
        protected abstract Task<(int Id, string File)?> FindExistingByHash(byte[] hash);
        protected abstract Task<bool?> CheckIfSourceIsBetter(string source, string destination);
        protected virtual async Task AfterImport(ImportResult import) { return; }
        protected virtual async Task<string?> CreateThumbnail(string file) { return null; }


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
                var existingObj = await FindExistingByHash(hash);
                

                if (existingObj == null)
                {
                    destination = RenameUntillUnique(destination);
                    MoveFile(source, destination);
                    result.RelativePath = Path.GetRelativePath(ExportFolder, destination);
                    result.SetResult(true, ImportStatus.ImportedUniqueFile);
                    await AfterImport(result);
                }
                else
                {
                    string existing = existingObj.Value.File;
                    result.MatchedWithId = existingObj.Value.Id;
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
                            DeleteFile(existing, result);
                            MoveFile(source, existing);
                            result.SetResult(true, ImportStatus.HashMatchKeptSource);
                            await AfterImport(result);
                        }
                        else
                        {
                            DeleteFile(source, result);
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

        public async void DeleteFile(string file, ImportResult result)
        {
            result.RemovedFileThumb = await CreateThumbnail(file);
            if (UseRecycleFolder)
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
