using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TFSW.Data
{
    public class ConfigurationManager
    {
        public SQLiteConnection Db { get; set; }
        public Configuration CurrentConfig => Configurations.Where(c => c.Activated).FirstOrDefault();
        public IEnumerable<Configuration> Configurations => Db.Table<Configuration>();

        public ConfigurationManager()
        {
            Db = new SQLiteConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "teamwork.db"));
            Db.CreateTable<Configuration>();
            ResetConfigs();
        }
        public void CreateConfig(Configuration config)
        {
            if (config.Activated)
            {
                foreach (var conf in Configurations.Where(c=> c.Activated))
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
            if (!Configurations.Any())
            {
                CreateConfig(new Configuration { Activated = true });
            }
        }

    }
}
