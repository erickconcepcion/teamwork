using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Params
{
    class ConfigParams
    {
        private IEnumerable<string> _parameters;
        private IEnumerable<string> _keys;
        public ConfigParams(IEnumerable<string> parameters, IEnumerable<string> keys)
        {
            _parameters = parameters.Where(p => p.StartsWith("--"));
            _keys = keys;
        }
        public bool Validate()
        {
            var ret = true;
            if (_parameters.Count() != 0)
            {
                Console.WriteLine("Supply at least one command or parameter.");
                ret = false;
            }
            var noParams = _parameters.Where(p => !_keys.Contains(p));
            if (noParams.Count()!=0)
            {
                Console.WriteLine($"Not recognized parameters {string.Join(",",noParams)}.");
                ret = false;
            }
            return ret;
        }
    }
}
