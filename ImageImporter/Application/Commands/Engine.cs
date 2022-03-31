using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageImporter.Application.Commands
{


    public class Engine
    {
        Dictionary<string, BaseCommand> commands = new Dictionary<string, BaseCommand>();
        private bool work = true;

        public Engine()
        {
            RegisterCommand(new Exit(this));
            RegisterCommand(new Help(this));
        }

        public void Execute()
        {
            Log("Console started!");
            PrintHelp();
            while(work)
            {
                string line = Console.ReadLine();
                int ind = line.IndexOf(' ');
                if (ind == -1)
                    ind = line.Length;

                string cmd = line.Substring(0, ind).ToLowerInvariant();
                string args = line.Substring(ind).Trim(' ');
                
                if (commands.TryGetValue(cmd, out BaseCommand? command))
                {
                    command?.Execute(args);
                }
                else
                {
                    Log("Invalid command");
                }
            }
        }

        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        public void RegisterCommand(string name, BaseCommand command)
        {
            command.SetEngine(this);
            commands.Add(name.ToLowerInvariant(), command);
        }

        public void RegisterCommand(BaseCommand command)
        {
            RegisterCommand(command.GetType().Name.ToLowerInvariant(), command);
        }

        public void PrintHelp()
        {
            foreach(var v in commands)
            {
                Log($" - {v.Key}");
            }
        }

        public void Exit()
        {
            work = false;
        }
    }
}
