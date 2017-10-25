using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kelor.Commands;

namespace Kelor
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = typeof(IKelorCommand).Assembly.GetTypes()
                                                         .Where(t => t.IsAbstract == false &&
                                                                     t.BaseType != null &&
                                                                     t.BaseType == typeof(KelorCommand))
                                                         .Select(t => (IKelorCommand)Activator.CreateInstance(t))
                                                         .ToArray();

            if (!args.Any())
            {
                Console.WriteLine("List of commands");
                Console.WriteLine("");
                foreach (var command in commands)
                {
                    Console.WriteLine("{0}: {1}", command.Code, command.Description);
                }
                return;
            }

            var localizedCommand = commands.FirstOrDefault(c => args.Contains(c.Code));
            if (localizedCommand == null)
            {
                Console.WriteLine("kelor: '{0}' is not a kelor command. See kelex -help", args.FirstOrDefault());
                return;
            }

            Console.WriteLine("");
            localizedCommand.Execute(args);
        }
    }
}
