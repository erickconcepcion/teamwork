using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Data;

namespace TFSW.Logics
{
    public class WorkItemManager: BaseManager<WorkItemHierarchy>
    {
        private readonly ConfigurationManager _configManager;
        private readonly AzureDevopsClient _azureDevopsClient;
        public WorkItemManager(ConfigurationManager configManager)
        {
            _configManager = configManager;
            _azureDevopsClient = new AzureDevopsClient(_configManager.CurrentConfig, true);
        }
        public async void CreateHierarchy(IEnumerable<WorkItemHierarchy>hierarchy, string hierarchyName)
        {
            var tasks = await Task.WhenAll(new Task<IEnumerable<WorkReference>>[]{
                _azureDevopsClient.GetWorkItemRelationTypesReference(),
                _azureDevopsClient.GetWorkItemTypesReference()
            });
            var relations = tasks[0];
            var types = tasks[1];
            foreach (var item in hierarchy)
            {
                var relation = relations.Where(r => r.Name == item.HierarchyType).FirstOrDefault();
                var type = types.Where(r => r.Name == item.WorkItemType).FirstOrDefault();
                if (relation is not null)
                {
                    item.WorkRelationshipType = relation?.ReferenceName ?? relations.Where(r => r.Name == "Child").FirstOrDefault().ReferenceName;
                    item.WorkItemType = type?.Name ?? "Task";
                    item.HierarchyName = hierarchyName;
                    Db.Insert(item);
                }
                else
                {
                    Console.WriteLine($"Could not store workitem template {item.Title}. {item.HierarchyType} Relation does not exists.");
                }
            }
        }
        public void DeleteHierarchy(string hierarchyName)
        {
            foreach (var item in All.Where(w => w.HierarchyName == hierarchyName))
            {
                Db.Delete(item);
            }
        }
    }
}
