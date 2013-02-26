using System;
using System.Configuration;

namespace TelChina.AF.Sys.Configuration
{
    public sealed class StorageCollection : ConfigurationElementCollection  
    {
        public StorageCollection()
        {
            AddElement settiongElement = (AddElement)CreateNewElement();

        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AddElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((AddElement)element).AppName;
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


        /// <summary>
        /// 结构评审以后将setting改成Add
        /// Modifier：zqy
        /// Time：2012-4-27
        /// </summary>
        protected override string ElementName
        {
            get { return "add"; }
        }

        public new int Count
        {
            get { return base.Count; }
        }


        public AddElement this[int index]
        {
            get
            {
                return (AddElement)BaseGet(index);
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

        new public AddElement this[string Name]
        {
            get
            {
                return (AddElement)BaseGet(Name);
            }
        }

        public int IndexOf(AddElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(AddElement element)
        {
            BaseAdd(element);
        }

        public void Remove(AddElement element)
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
