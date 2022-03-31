namespace ImageImporter.Application.Commands
{
    public class Help : BaseCommand
    {
        protected Engine Engine { get; }

        public Help(Engine engine)
        {
            Engine = engine;
        }

        public override void Execute(string args)
        {
            Engine.PrintHelp();
        }
    }

}
