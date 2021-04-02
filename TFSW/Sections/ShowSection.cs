using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Data;
using TFSW.Logics;
using TFSW.Params;
using TFSW.Utils;

namespace TFSW.Sections
{
    public class ShowSection: BaseSection
    {
        public ShowSection()
        {
            SectionName = "show";
        }
        private Dictionary<string, Action> Actions => new Dictionary<string, Action>()
        {
            { "projects", ()=> Console.WriteLine(
                new AzureDevopsClient(_config, setProjectClient: true)
                .GetProjects().Result.ToStringProjectTable()) },
            { "relationships", ()=> Console.WriteLine(
                new AzureDevopsClient(_config, true).GetWorkItemRelationTypes().Result.
                ToStringRelTable()) },
            { "types", ()=> Console.WriteLine(new AzureDevopsClient(_config, true).GetWorkItemTypes().Result
                .ToStringTypeTable()) },
            { "configs", ()=> Console.WriteLine(new ConfigurationManager().All.ToConfigTable()) }
        };
        public override void Execute(IEnumerable<string> parameters)
        {
            if (new ShowParams(parameters, Actions.Keys.AsEnumerable()).Validate())
            {
                Actions[parameters.FirstOrDefault()]();
            }
        }
    }
}
