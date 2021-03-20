using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Params
{
    class ShowParams
    {
        private IEnumerable<string> _parameters;
        private IEnumerable<string> _keys;
        public ShowParams(IEnumerable<string> parameters, IEnumerable<string> keys)
        {
            _parameters = parameters;
            _keys = keys;
        }
        public bool Validate()
        {
            var ret = true;
            if (_parameters.Count()!=1)
            {
                Console.WriteLine("Feature only support one parameter.");
                ret = false;
            }
            var featureName = _parameters.FirstOrDefault();
            if (!_keys.Any(k => k== featureName.ToLower()))
            {
                Console.WriteLine($"Feature name \"{featureName}\" not found.");
                ret = false;
            }
            return ret;
        }
    }
}
