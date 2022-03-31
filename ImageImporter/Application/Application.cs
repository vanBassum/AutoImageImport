using ImageImporter.Application;
using ImageImporter.Application.Commands;
using ImageImporter.Models;
using System.Diagnostics;

public class Application
{
    private AppDBContext AppDBContext { get; set; }
    private AppSettings AppSettings { get; set; }
    public Application(AppDBContext appDBContext, AppSettings appSettings)
    {
        AppDBContext = appDBContext;
        AppSettings = appSettings;
    }

    public void Execute()
    {
        Engine engine = new Engine();
        engine.RegisterCommand(new Import(AppSettings, AppDBContext));
        engine.RegisterCommand(new Hamming(AppSettings, AppDBContext));
        engine.Execute();
    }
}



