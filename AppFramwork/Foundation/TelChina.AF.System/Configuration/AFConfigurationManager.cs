using System;
using System.Configuration;
using System.IO;
using BaseConfig = System.Configuration;

namespace TelChina.AF.Sys.Configuration
{
    public class AFConfigurationManager : ConfigurationSection
    {
        private static bool isInited;
        /// <summary>
        /// 平台配置文件名称
        /// </summary>
        public const string CONFIGFILENAME = "Biz.config";
        /// <summary>
        /// 配置信息对象
        /// </summary>
        private static BaseConfig.Configuration config;

        /// <summary>
        /// 根据平台本身的配置文件 映射生成配置信息对象
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static void Setup()
        {
            //避免重复加载配置
            if (isInited) return;

            var fileMap = new ExeConfigurationFileMap();
            //string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(CONFIGFILENAME));
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIGFILENAME);
            fileMap.ExeConfigFilename = strPath;

            if (!File.Exists(strPath))
            {
                throw new FileNotFoundException(strPath + " 文件不存在");
            }

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
           
            isInited = true;

            //TODO：读取配置文件
        }

        private static PLGroup plGroup;
        public static PLGroup PLGroup
        {
            get
            {
                if (plGroup == null)
                {
                    // 如果使用上面已经定义好的静态的config得到的字段就是我们自己定义的config文件Biz.config
                    plGroup = config.GetSection("plGroup") as PLGroup;
                    // 如果使用下面的语句，使用的就是系统默认的App.config配置文件。这里是App.config的接口
                    //plSection = ConfigurationManager.GetSection("plGroup") as PLSection;
                }
                return plGroup;
            }
        }
        private static SVGroup _svGroup;
        public static SVGroup SVGroup
        {
            get { return _svGroup ?? (_svGroup = config.GetSection("svGroup") as SVGroup); }
        }
    }
}
