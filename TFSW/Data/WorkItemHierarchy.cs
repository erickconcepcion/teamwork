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
        //Title with format
        public string Title { get; set; }
        //name of relationship Child, Relatet ETC
        public string HierarchyType { get; set; }
        //US Task ETC
        public string WorkItemType { get; set; }
        //Reference of HierarchyType
        public string WorkRelationshipType { get; set; }
        public string HierarchyName { get; set; }
    }
}
