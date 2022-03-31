using ImageImporter.Models;

namespace ImageImporter.Application.Commands
{
    public class Import : BaseCommand
    {
        public Import(ConsoleCommands consoleCommands, AppSettings appSettings, AppDBContext appDBContext) : base(consoleCommands, appSettings, appDBContext)
        {
        }

        public override void Execute(string args)
        {
            foreach (string file in Directory.GetFiles(AppSettings.ImportFolder, "", SearchOption.AllDirectories))
            {
                Picture? picture = Picture.Generate(AppSettings, file);
                if(picture?.RelativePath != null)
                {
                    if (AppDBContext.Pictures.Any(a => a.HashA == picture.HashA))
                    {
                        ConsoleCommands.Log($"Not imported, duplicate hash. {picture.ToString()}");
                    }
                    else
                    {
                        string savePath = Path.Combine(AppSettings.ImagesFolder, picture.RelativePath);

                        if(File.Exists(savePath))
                        {
                            ConsoleCommands.Log($"Not imported, duplicate filename. {picture.ToString()}");
                        }
                        else
                        {
                            string? directory = Path.GetDirectoryName(savePath);
                            if(directory != null)
                            {
                                Directory.CreateDirectory(directory);
                                File.Move(file, savePath);
                                ConsoleCommands.Log($"Imported. {picture.ToString()}");
                                AppDBContext.Add(picture);
                                AppDBContext.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}
