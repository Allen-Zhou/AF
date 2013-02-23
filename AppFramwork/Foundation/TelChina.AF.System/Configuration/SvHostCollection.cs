using System;
using System.Configuration;

namespace TelChina.AF.Sys.Configuration
{
    [ConfigurationCollection(typeof(HostSettings))]
    public sealed class SvHostCollection : ConfigurationElementCollection
    {
        private static ConfigurationPropertyCollection _properties;


        static SvHostCollection()
        {
            // Property initialization 
            _properties = new ConfigurationPropertyCollection();
        }

        public SvHostCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HostSettings();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((HostSettings)element).AppName;
        }

        public new string AddElementName
        {
            get
            { return base.AddElementName; }

            set
            { base.AddElementName = value; }
        }

        public new string ClearElementName
        {
            get
            { return base.ClearElementName; }

            set
            { base.AddElementName = value; }

        }

        public new string RemoveElementName
        {
            get
            { return base.RemoveElementName; }
        }


        protected override string ElementName
        {
            get { return "add"; }
        }

        public new int Count
        {
            get { return base.Count; }
        }


        public HostSettings this[int index]
        {
            get
            {
                return (HostSettings)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public HostSettings this[string appName]
        {
            get
            {
                return (HostSettings)BaseGet(appName);
            }
        }

        public int IndexOf(HostSettings element)
        {
            return BaseIndexOf(element);
        }

        public void Add(HostSettings element)
        {
            BaseAdd(element);
        }

        public void Remove(HostSettings element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.AppName);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string client)
        {
            BaseRemove(client);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
