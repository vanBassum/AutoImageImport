namespace ImageImporter.Application.Commands
{
    public class Help : BaseCommand
    {
        public Help(ConsoleCommands consoleCommands, AppSettings appSettings, AppDBContext appDBContext) : base(consoleCommands, appSettings, appDBContext)
        {
        }

        public override void Execute(string args)
        {
            ConsoleCommands.PrintHelp();
        }
    }

}
