using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Data
{
    public class WorkItemHierarchy
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string HierarchyType { get; set; }
        public string WorkItemType { get; set; }
        public string WorkRelationshipType { get; set; }
        public string HierarchyName { get; set; }
    }
}
