using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelor.Commands
{
    public class Unzip : KelorCommand
    {
        private IEnumerable<KelorCommandParameter> parameters;

        public Unzip()
        {
            parameters = new KelorCommandParameter[] { };
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
            ZipFile.ExtractToDirectory("", "");
        }
    }
}
