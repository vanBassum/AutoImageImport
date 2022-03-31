namespace ImageImporter.Application.Commands
{
    public abstract class BaseCommand
    {
        protected ConsoleCommands ConsoleCommands { get; }
        protected AppSettings AppSettings { get; }
        protected AppDBContext AppDBContext { get; }

        protected BaseCommand(ConsoleCommands consoleCommands, AppSettings appSettings, AppDBContext appDBContext)
        {
            ConsoleCommands = consoleCommands;
            AppSettings = appSettings;
            AppDBContext = appDBContext;
        }

        public abstract void Execute(string args);
    }

}
