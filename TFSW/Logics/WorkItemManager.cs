using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Data;
using System.Text.Json;
using System.IO;

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
        private async Task<(IEnumerable<WorkReference> relations, IEnumerable<WorkReference> types)> GetReferences()
        {
            var tasks = await Task.WhenAll(new Task<IEnumerable<WorkReference>>[]{
                _azureDevopsClient.GetWorkItemRelationTypesReference(),
                _azureDevopsClient.GetWorkItemTypesReference()
            });
            return (tasks[0], tasks[1]);
        }
        private int InsertHierarchy(WorkItemHierarchy item, string relationType, string itemType, string hierarchyName)
        {
            item.WorkRelationshipType = relationType;
            item.WorkItemType = itemType;
            item.HierarchyName = hierarchyName;
            return Db.Insert(item);
        }
        public async Task CreateHierarchy(IEnumerable<WorkItemHierarchy>hierarchy, string hierarchyName)
        {
            var references = await GetReferences();
            foreach (var item in hierarchy)
            {
                var relation = references.relations.Where(r => r.Name == item.HierarchyType).FirstOrDefault();
                var type = references.types.Where(r => r.Name == item.WorkItemType).FirstOrDefault();
                if (relation is null)
                {
                    Console.WriteLine($"Could not store workitem template {item.Title}. {item.HierarchyType} Relation does not exists.");
                    return;
                }
                InsertHierarchy(item, relation?.ReferenceName ?? references.relations.Where(r => r.Name == "Child").FirstOrDefault().ReferenceName,
                        type?.Name ?? "Task", hierarchyName);
            }
        }
        public async Task CreateHierarchyFromJson(string hierarchyName, string jsonHierarchy = null, string jsonPath=null)
        {
            Console.WriteLine($"{hierarchyName}, {jsonHierarchy}, {jsonPath}");
            var json = string.IsNullOrEmpty(jsonHierarchy) ? File.ReadAllText(jsonPath) : jsonHierarchy;
            var hierarchies = JsonSerializer.Deserialize<ICollection<WorkItemHierarchy>>(jsonHierarchy);
            if (hierarchies is null) throw new ArgumentNullException("Json string or file are required");
            foreach (var item in hierarchies)
            {
                Console.WriteLine(item.Title);
            }
            await CreateHierarchy(hierarchies, hierarchyName);
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
