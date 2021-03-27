using System;
using System.Collections.Generic;
using System.Linq;

namespace TFSW.Params
{
    public class CommandValidator
    {
        private IEnumerable<string> _parameters;
        private IEnumerable<string> _keys;
        public CommandValidator(IEnumerable<string> parameters, IEnumerable<string> keys)
        {
            _parameters = parameters;
            _keys = keys;
        }
        public bool Validate()
        {
            var ret = true;
            if (_parameters.Count() == 0)
            {
                Console.WriteLine("No command provided.");
                ret = false;
            }
            var commandName = _parameters.FirstOrDefault();
            if (!_keys.Any(k => k == commandName.ToLower()))
            {
                Console.WriteLine($"Command name \"{commandName}\" not found.");
                ret = false;
            }
            return ret;
        }
    }
}