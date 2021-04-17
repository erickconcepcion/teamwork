using System.Collections.Generic;
using TFSW.Sections;
using System.Linq;
using TFSW.Params;

/*args = new string[] { "hierarchy",
    "-n",
    "taskus",
    "-c",
    "[{\"Title\": \"A title {1}{2}\",\"HierarchyType\": \"Child\",\"WorkItemType\": \"Task\"},{\"Title\": \"A US title {1}{2}\",\"HierarchyType\": \"Child\",\"WorkItemType\": \"User Story\"}]"
};*/
var features = new List<BaseSection>
{
    new ShowSection(),
    new ConfigSection(),
    new HierarchySection()
};
var featureRouter = features.ToDictionary(k => k.SectionName, v=> v);
if (new CommandValidator(args, featureRouter.Keys).Validate())
{
    featureRouter[args[0]].Execute(args.Skip(1));
}
