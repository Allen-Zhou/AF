using System;
using System.Configuration;
using System.Xml;

namespace TelChina.AF.Sys.Configuration
{
    public sealed class SVGroup : ConfigurationSection
    {

        [ConfigurationProperty("svHosts")]
        public SvHostCollection Hosts
        {
            get
            {
                return (SvHostCollection)base["svHosts"];
            }
            set
            {
                this["svHosts"] = value;
            }
        }

        //[ConfigurationProperty("default")]
        //public SVDefaultElement DefaultValue
        //{
        //    get
        //    {
        //        return (SVDefaultElement)base["default"];
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
