using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Data
{
    public class TeamworkDb: SQLiteConnection
    {
        public TeamworkDb(): base(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "teamwork.db"))
        {
            CreateTable<Configuration>();
            CreateTable<WorkItemHierarchy>();
        }
    }
}
