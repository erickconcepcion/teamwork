using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Data
{
    public class Configuration
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ServerUrl { get; set; }
        public string PersonalToken { get; set; }
        public string User { get; set; }
        public string Domain { get; set; }
        public Guid Project { get; set; }
        public bool Activated { get; set; }
    }
}
