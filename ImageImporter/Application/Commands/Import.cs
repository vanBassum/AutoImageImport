using ImageImporter.Models;
using System.Diagnostics;

namespace ImageImporter.Application.Commands
{
    public class Import : BaseCommand
    {
        private AppSettings AppSettings { get; }
        private AppDBContext AppDBContext { get; }

        public Import(AppSettings appSettings, AppDBContext appDBContext)
        {
            AppSettings = appSettings;
            AppDBContext = appDBContext;
        }

        class ImportStatus
        {
            public bool Imported
        }

        public override void Execute(string args)
        {
            foreach (string file in Directory.GetFiles(AppSettings.ImportFolder, "", SearchOption.AllDirectories))
            {
                string relativeFile = Path.GetRelativePath(AppSettings.ImportFolder, file);
                string savePath = Path.Combine(AppSettings.ImagesFolder, relativeFile);
                Stopwatch stopwatch = Stopwatch.StartNew();
                Status status = Status.Untouched;


                if (File.Exists(savePath))
                {

                    stopwatch.Stop();
                    Engine.Log(
                        ("Not imported. Duplicate filename", 35),
                        ($"{stopwatch.ElapsedMilliseconds}ms", 8)
                        );
                }
                else
                {
                    Picture? picture = Picture.Generate(AppSettings, file);
                    if (picture?.RelativePath != null)
                    {
                        Picture? matchingPicture = AppDBContext.Pictures.FirstOrDefault(a => a.HashA == picture.HashA);
                        if (matchingPicture?.RelativePath != null)
                        {
                            stopwatch.Stop();
                            Engine.Log(
                                ("Not imported. Duplicate hash", 35),
                                ($"{stopwatch.ElapsedMilliseconds}ms", 8),
                                (picture.RelativePath, 64),
                                (matchingPicture.RelativePath, 64)
                                );
                        }
                        else
                        {

                            string? directory = Path.GetDirectoryName(savePath);
                            if (directory != null)
                            {
                                Directory.CreateDirectory(directory);
                                File.Move(file, savePath);
                                stopwatch.Stop();
                                AppDBContext.Add(picture);
                                AppDBContext.SaveChanges();
                                Engine.Log(
                                    ("Imported.", 35),
                                    ($"{stopwatch.ElapsedMilliseconds}ms", 8),
                                    (Convert.ToHexString(picture.HashA), 64),
                                    (picture.RelativePath, 64)
                                    );
                            }
                        }
                    }
                    else
                    {
                        stopwatch.Stop();
                        Engine.Log(
                                    ("Not imported. File not supported.", 35),
                                    ($"{stopwatch.ElapsedMilliseconds}ms", 8),
                                    (relativeFile, 64)
                                    );
                    }
                }

                stopwatch.Stop();
            }
        }
    }
}
