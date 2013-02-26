using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace TelChina.AF.Util.Common
{
    public class ConfigHelper
    {
        /// <summary>
        /// 取AppSettings节中指定键的值
        /// </summary>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static string GetConfigValue(string configKey)
        {
            string result = "";
            if (!string.IsNullOrEmpty(configKey))
            {
                result = (System.Configuration.ConfigurationManager.AppSettings[configKey]);
            }
            return result;
        }
        public static Configuration LoadBindingConfigs()
        {
            var map = new ExeConfigurationFileMap();
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Biz.config");
            map.ExeConfigFilename = filePath;
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }
        public static Configuration LoadAppConfig()
        {
            return System.Configuration.ConfigurationManager.OpenExeConfiguration("");
        }
    }
}
