using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Data;
using TFSW.Logics;

namespace TFSW.Sections
{
    public abstract class BaseSection: ISection
    {
        public string SectionName { get; protected set; }
        protected OptionSet _options;
        protected readonly Configuration _config;
        protected bool _help;
        public BaseSection()
        {
            _config = new ConfigurationManager().CurrentConfig;
            _options = new OptionSet();
        }
        public abstract void Execute(IEnumerable<string> parameters);
        private void ShowHelp()
        {
            Console.WriteLine($"Usage: teamwork {SectionName} [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            _options.WriteOptionDescriptions(Console.Out);
        }
        protected bool ExecuteParams(IEnumerable<string> args)
        {
            try
            {
                _options.Parse(args);
                if (_help) ShowHelp();
                return true;
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Try `teamwork {SectionName} --help' for more information.");
            }
            return false;

        }
    }
}
