using System;
using System.Configuration;
using System.Xml;

namespace TelChina.AF.Sys.Configuration
{
    public sealed class PLGroup : ConfigurationSection
    {

        [ConfigurationProperty("provider",
            IsRequired = true,
            IsKey = false)]
        public String provider
        {
            get
            {
                return (String)base["provider"];
            }
            set
            {
                this["provider"] = value;
            }
        }

        /// <summary>
        /// 结构评审以后将文件里的settings改成Storages
        /// 但是PLSection的settings属性名称是否需要修改
        /// Modifier：zqy
        /// Time：2012-4-27
        /// </summary>
        [ConfigurationProperty("storages")]
        public StorageCollection Storages
        {
            get
            {

                return (StorageCollection)base["storages"];
            }
            set
            {
                this["storages"] = value;
            }
        }

        /// <summary>
        /// default配置
        /// Modifier：zqy
        /// Time：2012-7-16
        /// </summary>
        //[ConfigurationProperty("default")]
        //public PLDefaultElement DefaultValue
        //{
        //    get
        //    {

        //        return (PLDefaultElement)base["default"];
        //    }
        //    set
        //    {
        //        this["default"] = value;
        //    }
        //}

        protected override void DeserializeSection(
            XmlReader reader)
        {
            base.DeserializeSection(reader);
            // You can add custom processing code here.
        }

        protected override string SerializeSection(
            ConfigurationElement parentElement,
            string name, ConfigurationSaveMode saveMode)
        {
            string s = base.SerializeSection(parentElement, name, saveMode);
            // You can add custom processing code here.
            return s;
        }

    }
}
