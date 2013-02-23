using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Cfg;
using TelChina.AF.Sys.Configuration;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Test.DemoSV.Test
{
    internal class ConfigurationBuilder
    {
        private const string SERIALIZED_CFG = "configuration.bin";
        private const string CONFIGPATH = @"Config\EntityMapping";

        public Configuration Build()
        {
            Configuration cfg = LoadConfigurationFromFile();
            if (cfg == null)
            {

                foreach (TelChina.AF.Sys.Configuration.AddElement item in AFConfigurationManager.PLGroup.Storages)
                {
                    var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, item.FileName);
                    cfg = new NHibernate.Cfg.Configuration();
                    cfg.Configure(fileName);

                    //设置实体映射文件关联
                    var mappingFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIGPATH);
                    cfg.AddDirectory(new System.IO.DirectoryInfo(mappingFileName));
                }

                SaveConfigurationToFile(cfg);
            }
            return cfg;
        }

        private Configuration LoadConfigurationFromFile()
        {
            if (!IsConfigurationFileValid())
                return null;
            try
            {
                using (var file = File.Open(SERIALIZED_CFG, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    return bf.Deserialize(file) as Configuration;
                }
            }
            catch (Exception)
            {
                // Something went wrong
                // Just build a new one
                return null;
            }
        }

        private bool IsConfigurationFileValid()
        {
            // If we don't have a cached config, 
            // force a new one to be built
            if (!File.Exists(SERIALIZED_CFG))
                return false;
            var configInfo = new FileInfo(SERIALIZED_CFG);
            var asm = Assembly.GetExecutingAssembly();
            if (asm.Location == null)
                return false;
            // If the assembly is newer, 
            // the serialized config is stale
            var asmInfo = new FileInfo(asm.Location);
            if (asmInfo.LastWriteTime > configInfo.LastWriteTime)
                return false;
            // If the app.config is newer, 
            // the serialized config is stale
            var appDomain = AppDomain.CurrentDomain;
            var appConfigPath = appDomain.SetupInformation.
          ConfigurationFile;
            var appConfigInfo = new FileInfo(appConfigPath);
            if (appConfigInfo.LastWriteTime > configInfo.LastWriteTime)
                return false;
            // It's still fresh
            return true;
        }
        private void SaveConfigurationToFile(Configuration cfg)
        {
            using (var file = File.Open(SERIALIZED_CFG, FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, cfg);
            }
        }



    }
}
