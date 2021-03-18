using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Data;

namespace TFSW.Logics
{
    public class BaseManager<T> where T : class, new()
    {
        public TeamworkDb Db { get; private set; }
        public IEnumerable<T> All => Db.Table<T>();
        public BaseManager()
        {
            Db = new TeamworkDb();
        }
    }
}
