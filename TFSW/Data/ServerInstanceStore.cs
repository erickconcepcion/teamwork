using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Data
{
    class ServerInstanceStore
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ServerUrl { get; set; }
        public string Alias { get; set; }
    }
}
