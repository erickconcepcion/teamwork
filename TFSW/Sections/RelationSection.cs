using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Logics;

namespace TFSW.Sections
{
    public class RelationSection : BaseSection
    {
        public RelationSection()
        {
            SectionName = "relation";
            InitParamsActions();
        }
        private WorkItemManager _workItemManager;
        private IEnumerable<int> ids;
        private string name;
        private void InitParamsActions()
        {
            _options = new OptionSet()
            {
                { "n|name=", "Hierarchy name (look at )", v=> name = v },
                { "i|ids=", "Work item ids in comma separated ex: 1,2,25", v=> ids = v.Split(',').Select(i=> int.Parse(i)) },
            };
        }
        public override void Execute(IEnumerable<string> parameters)
        {
            _workItemManager = new WorkItemManager(new ConfigurationManager());
            ExecuteParams(parameters);
            if (!string.IsNullOrEmpty(name) && ids.Count()!=0)
            {
                Console.WriteLine("Nesting work items");
                _workItemManager.NestedAllWorkItem(name, ids).Wait();
                Console.WriteLine("Nested work items Completed");
            }

        }
    }
}
