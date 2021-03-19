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
        public void CreateHierarchy(IEnumerable<WorkItemHierarchy>hierarchy, string hierarchyName)
        {
            var relations = _azureDevopsClient.GetWorkItemRelationTypes().Result;
            foreach (var item in hierarchy)
            {
                var relation = relations.Where(r => r.Name == item.HierarchyType);
                if (relation is not null)
                {
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
