﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelor.Commands
{
    public abstract class KelorCommand : IKelorCommand
    {
        public abstract string Code { get; }
        public abstract string Description { get; }
        public abstract IEnumerable<IKelorCommandParameter> Parameters { get; }

        public void Execute(string[] args)
        {
            if (!(args.Length > 1) && Parameters.Any(p => p.Required))
            {
                ShowParameters();
                return;
            }

            FillParameters(args);

            var requiredParametersNotFilled = Parameters.Where(p => p.Required && String.IsNullOrWhiteSpace(p.Value));

            if (requiredParametersNotFilled.Any())
            {
                foreach (var parameter in requiredParametersNotFilled)
                {
                    Console.WriteLine("{0}: {1}", parameter.Key, parameter.ErrorRequiredMessage);
                }

                return;
            }

            Execute();
        }

        protected abstract void Execute();

        private void FillParameters(string[] args)
        {
            var keysAndValues = GetKeysAndValues(args);

            for (int i = 0; i < keysAndValues.Count; i++)
            {
                var arg = keysAndValues[i];
                var parameter = Parameters.FirstOrDefault(p => p.Key.Equals(arg, StringComparison.InvariantCultureIgnoreCase));
                if (parameter == null)
                {
                    Console.WriteLine("kelor {0} is not a command. See 'kelor -h'", arg);
                    break;
                }

                parameter.Declared = true;

                i++;

                if (parameter.NotRequiredAValue) continue;

                if (!(i < keysAndValues.Count))
                {
                    Console.WriteLine("{0}: {1}", parameter.Key, parameter.ErrorRequiredMessage);
                    break;
                }

                parameter.Value = keysAndValues[i];
            }
        }

        private static List<string> GetKeysAndValues(string[] args)
        {
            var parameterInArgs = new List<string>();
            for (int i = 1; i < args.Length; i++)
            {
                parameterInArgs.Add(args[i]);
            }

            return parameterInArgs;
        }

        private void ShowParameters()
        {
            Console.WriteLine("List of parameters");
            Console.WriteLine("");
            foreach (var parameter in Parameters)
            {
                Console.WriteLine("{0}: {1} Required: {2}", parameter.Key, parameter.Description, parameter.Required);
            }
        }
    }
}
