using System.Collections.Generic;
using TFSW.Sections;
using System.Linq;
using TFSW.Params;

var features = new List<BaseSection>
{
    new ShowSection(),
    new ConfigSection()
};
var featureRouter = features.ToDictionary(k => k.SectionName, v=> v);
if (new CommandValidator(args, featureRouter.Keys).Validate())
{
    featureRouter[args[0]].Execute(args.Skip(1));
}
