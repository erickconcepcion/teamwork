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
        public WorkItemManager(ConfigurationManager configManager)
        {
            _configManager = configManager;
        }
        public void CreateHierarchy(IEnumerable<WorkItemHierarchy>hierarchy, string hierarchyName)
        {
            foreach (var item in hierarchy)
            {
                item.HierarchyName = hierarchyName;
                Db.Insert(item);
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
