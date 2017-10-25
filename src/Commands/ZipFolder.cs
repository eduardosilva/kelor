using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelor.Commands
{
    public class ZipFolder : KelorCommand
    {
        private IEnumerable<KelorCommandParameter> parameters;

        private string pathSource;
        private string pathTo;
        private string zipName;

        public ZipFolder()
        {
            parameters = new[]
            {
                new KelorCommandParameter { Key = Constants.SOURCE, Description = "Folder to compress", Required = true },
                new KelorCommandParameter { Key = Constants.NAME, Description = "Zip file name" },
                new KelorCommandParameter { Key = Constants.TO, Description = "Zip file destination" },
            };
        }

        public override string Code
        {
            get { return "zipfolder"; }
        }

        public override string Description
        {
            get{ return "Compress a folder"; }
        }

        public override IEnumerable<IKelorCommandParameter> Parameters
        {
            get{ return parameters; }
        }

        protected override void Execute()
        {
            pathSource = parameters.FirstOrDefault(p => p.Key == Constants.SOURCE).Value;
            pathTo = parameters.FirstOrDefault(p => p.Key == Constants.TO)?.Value;
            zipName = parameters.FirstOrDefault(p => p.Key == Constants.NAME)?.Value;

            if (!Directory.Exists(pathSource))
            {
                Console.WriteLine("folder to zip does not exists");
                return;
            }

            if (!String.IsNullOrEmpty(pathTo) && !Directory.Exists(pathSource))
            {
                Console.WriteLine("path destination to zip does not exists");
                return;
            }

            if (String.IsNullOrEmpty(pathTo))
                pathTo = Path.GetDirectoryName(pathSource);

            if (String.IsNullOrWhiteSpace(zipName))
                zipName = Path.GetFileNameWithoutExtension(pathSource) + ".zip";

            if (!(Path.GetExtension(zipName) == ".zip"))
                throw new InvalidOperationException($"{zipName} has a invalid zip extension");

            var fullPath = Path.Combine(pathTo, zipName);

            Console.WriteLine("Compressing folder...");
            ZipFile.CreateFromDirectory(pathSource, fullPath);

            Console.WriteLine($"{fullPath} has generated successfully!");
        }
    }
}