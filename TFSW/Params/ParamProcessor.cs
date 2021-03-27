using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Params
{
    class ParamProcessor: Dictionary<string, CommandValidator>, IDictionary<string, CommandValidator>
    {
    }
}
