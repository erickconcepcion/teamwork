using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TFSW.Data;

namespace TFSW.Logics
{
    public class ConfigurationManager: BaseManager<Configuration>
    {
        public Configuration CurrentConfig => All.Where(c => c.Activated).FirstOrDefault();

        public ConfigurationManager()
        {
            ResetConfigs();
        }
        public void CreateConfig(Configuration config)
        {
            config.Id = 0;
            if (config.Activated)
            {
                foreach (var conf in All.Where(c=> c.Activated))
                {
                    conf.Activated = false;
                    Db.Update(conf);
                }
            }
            Db.Insert(config);
        }
        public void UpdateConfig(Configuration config)
        {
            Db.Update(config);
        }
        public void DeleteConfig()
        {
            Db.Delete(CurrentConfig);
            ResetConfigs();
        }
        private void ResetConfigs()
        {
            if (!All.Any())
            {
                CreateConfig(new Configuration { Activated = true });
            }
        }

    }
}
