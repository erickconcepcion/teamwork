using System.Collections.Generic;
using TFSW.Sections;
using System.Linq;
using TFSW.Params;

/*args = new string[] {};*/
var features = new List<BaseSection>
{
    new ShowSection(),
    new ConfigSection(),
    new HierarchySection(),
    new RelationSection()
};
var featureRouter = features.ToDictionary(k => k.SectionName, v=> v);
if (new CommandValidator(args, featureRouter.Keys).Validate())
{
    featureRouter[args[0]].Execute(args.Skip(1));
}
