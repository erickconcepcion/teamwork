using NDesk.Options;
using System.Collections.Generic;
using TFSW.Logics;

namespace TFSW.Sections
{
    public class HierarchySection : BaseSection
    {
        public HierarchySection()
        {
            SectionName = "hierarchy";
            _workItemManager = new WorkItemManager(new ConfigurationManager());
            InitParamsActions();
        }

        private readonly WorkItemManager _workItemManager;
        private string jsonString;
        private string name;
        private string jsonPath;
        private bool delete;
        private void InitParamsActions()
        {
            _options = new OptionSet()
            {
                { "n|name=", "Name of hierarchy for execute a relation", v=> name = v },
                { "c|create=", "Json string to create multiple work items hierarchy", v=> jsonString = v },
                { "p|path=", "Path that contain a json file to create multiple work items hierarchy", v=> jsonPath = v },
                { "d|delete", "Flag to delete a hierarchy", v=> delete = v!=null }
                
            };
        }
        /* Powershell string syntax
        "[{\`"Title\`": \`"A title {1}{2}\`",\`"HierarchyType\`": \`"Child\`",\`"WorkItemType\`": \`"Task\`"},{\`"Title\`": \`"A US title {1}{2}\`",\`"HierarchyType\`": \`"Child\`",\`"WorkItemType\`": \`"User Story\`"}]"
        "[{\"Title\": \"A title {1}{2}\",\"HierarchyType\": \"Child\",\"WorkItemType\": \"Task\"},{\"Title\": \"A US title {1}{2}\",\"HierarchyType\": \"Child\",\"WorkItemType\": \"User Story\"}]"
         */

        public override void Execute(IEnumerable<string> parameters)
        {
            ExecuteParams(parameters);
            if (!string.IsNullOrEmpty(name))
            {
                if (delete)
                {
                    _workItemManager.DeleteHierarchy(name);
                    return;
                }
                if(jsonPath is not null || jsonString is not null)
                    _workItemManager.CreateHierarchyFromJson(name, jsonString, jsonPath).Wait();
            }
        }
    }
}
