namespace ImageImporter.Application.Commands
{
    public abstract class BaseCommand
    {
        protected Engine Engine { get; private set; }
        //protected AppSettings AppSettings { get; }
        //protected AppDBContext AppDBContext { get; }
        //
        //protected BaseCommand(Engine consoleCommands, AppSettings appSettings, AppDBContext appDBContext)
        //{
        //    Engine = consoleCommands;
        //    AppSettings = appSettings;
        //    AppDBContext = appDBContext;
        //}

        public abstract void Execute(string args);

        public void SetEngine(Engine engine)
        {
            Engine = engine;
        }
    }

}
