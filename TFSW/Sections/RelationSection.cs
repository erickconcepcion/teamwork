using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Sections
{
    public class RelationSection : BaseSection
    {
        public RelationSection()
        {
            SectionName = "relation";
            InitParamsActions();
        }
        private void InitParamsActions()
        {
            _options = new OptionSet()
            {
                { "o|orgurl=", "Azure Devops/TFS organization or collection url", v=> _config.ServerUrl = v },
                { "t|personaltoken=", "Azure Devops/TFS Personal token generated on user settings used for auth", v=> _config.PersonalToken = v },
                { "u|user=", "Azure Devops/TFS user network credentials for on-prem auth", v=> _config.User = v },
                { "d|domain=", "Azure Devops/TFS domain name network credentials for on-prem auth", v=> _config.Domain = v },
                { "p|projectid=", "Azure Devops/TFS project GUID for actual configuration. Run \"teamwork show projects\" commands to see all project",
                    v=> _config.Project = Guid.TryParse(v, out Guid placeholder) ? new Guid(v) : _config.Project },
                { "dc|domaincreds", "Azure Devops/TFS organization or collection url", v=> _config.IsDomainCreds = v!=null },
                { "h|help",  "show this message and exit",v => _help = v != null }
            };
        }
        public override void Execute(IEnumerable<string> parameters)
        {
            ExecuteParams(parameters);
            

        }
    }
}
