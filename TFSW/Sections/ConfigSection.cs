using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Data;
using TFSW.Logics;
using TFSW.Params;

namespace TFSW.Sections
{
    public class ConfigSection : BaseSection
    {
        private bool _create;
        public ConfigSection()
        {
            SectionName = "config";
            InitParamsActions();
        }
        private void InitParamsActions()
        {
            Guid placeholder;
            _create = false;
            _options = new OptionSet()
            {
                { "o|orgurl", "Azure Devops/TFS organization or collection url", v=> _config.ServerUrl = v },
                { "t|personaltoken", "Azure Devops/TFS Personal token generated on user settings used for auth", v=> Console.WriteLine() },
                { "u|user", "Azure Devops/TFS user network credentials for on-prem auth", v=> Console.WriteLine() },
                { "d|domain", "Azure Devops/TFS domain name network credentials for on-prem auth", v=> Console.WriteLine() },
                { "p|projectid", "Azure Devops/TFS project GUID for actual configuration. Run \"teamwork show projects\" commands to see all project",
                    v=> _config.Project = Guid.TryParse(v, out placeholder) ? new Guid(v) : _config.Project },
                { "dc|domaincreds", "Azure Devops/TFS organization or collection url", v=> _config.IsDomainCreds = v!=null },
                { "c|create", "Azure Devops/TFS organization or collection url", v=> _create= v!= null },
                { "h|help",  "show this message and exit",v => _help = v != null }
            };
        }
        public override void Execute(IEnumerable<string> parameters)
        {
            var manager = new ConfigurationManager();
            ExecuteParams(parameters);
            if (!_create)
            {
                manager.UpdateConfig(_config);
                return;
            }
            _config.Id = 0;
            manager.CreateConfig(_config);

        }
    }
}
