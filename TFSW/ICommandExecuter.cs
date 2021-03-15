using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW
{
    interface ICommandExecuter
    {
        void ExecuteCommand(params object[] parameters);
    }
}
