using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelor.Commands
{
    public class UnZip : KelorCommand
    {
        private IEnumerable<KelorCommandParameter> parameters;

        public UnZip()
        {
            parameters = new[]
            {
                new KelorCommandParameter { Key = Constants.SOURCE, Description = "Zip path", Required = true },
                new KelorCommandParameter { Key = Constants.TO, Description = "Files destination", Required = true }
            };
        }

        public override string Code
        {
            get { return "unzip"; }
        }

        public override string Description
        {
            get { return "Unzip a file"; }
        }

        public override IEnumerable<IKelorCommandParameter> Parameters
        {
            get { return parameters; }
        }

        protected override void Execute()
        {
            try
            {
                var zipFile = parameters.FirstOrDefault(p => p.Key == Constants.SOURCE).Value;
                var pathTo = parameters.FirstOrDefault(p => p.Key == Constants.TO).Value;

                if (File.Exists(zipFile))
                {
                    Console.WriteLine("zip file not found");
                    return;
                }

                if (!(Path.HasExtension(zipFile) && Path.GetExtension(zipFile) == ".zip"))
                {
                    Console.WriteLine("Invalid zip file");
                    return;
                }

                if (Directory.Exists(pathTo))
                {
                    Console.WriteLine("Destination files not found");
                    return;
                }

                ZipFile.ExtractToDirectory(zipFile, pathTo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}