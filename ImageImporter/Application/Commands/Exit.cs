namespace ImageImporter.Application.Commands
{
    public class Exit : BaseCommand
    {
        private Engine Engine { get; }

        public Exit(Engine engine)
        {
            Engine = engine;
        }

        public override void Execute(string args)
        {
            Engine.Exit();
        }
    }

}
