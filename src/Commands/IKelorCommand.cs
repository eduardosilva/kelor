using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelor.Commands
{
    public interface IKelorCommand
    {
        string Code { get; }
        string Description { get;  }
        IEnumerable<IKelorCommandParameter> Parameters { get;  }

        void Execute(string[] args);
    }
}
