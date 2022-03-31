namespace ImageImporter.Application.Commands
{
    public class Exit : BaseCommand
    {
        public Exit(ConsoleCommands consoleCommands, AppSettings appSettings, AppDBContext appDBContext) : base(consoleCommands, appSettings, appDBContext)
        {
        }

        public override void Execute(string args)
        {
            ConsoleCommands.Exit();
        }
    }

}
