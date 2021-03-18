using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Params
{
    class ConfigParams
    {
        private readonly string[] _parameters;                                       
        public ConfigParams(string[] parameters)
        {
            _parameters = parameters;
        }
    }
}
