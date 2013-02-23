using System;
using System.Configuration;
using System.Xml;

namespace TelChina.AF.Sys.Configuration
{
   public class AddElement : ConfigurationElement
    {
        /// <summary>
        /// ormType经讨论已经提出
        /// </summary>
        /// <param name="newAppName"></param>
        /// <param name="newFileName"></param>
        public AddElement(String newAppName, String newFileName)//, String ormType)
        {
            AppName = newAppName;
            FileName = newFileName;
            //OrmType = ormType;
        }

        // Default constructor, will use default values as defined
        // below.
        public AddElement()
        {
        }


       /// <summary>
       /// 可以对该属性配置正则表达式来对其进行约束
       /// ConfigurationProperty成员
       /// </summary>
        [ConfigurationProperty("appName", IsKey = true, IsRequired = true)]
        public string AppName
        {
            get { return (string)base["appName"]; }
            set { base["appName"] = value; }
        }

        [ConfigurationProperty("fileName", IsRequired = true)]
        public string FileName
        {
            get { return (string)base["fileName"]; }
            set { base["fileName"] = value; }
        }

        //[ConfigurationProperty("ormType", IsRequired = true)]
        //public string OrmType
        //{
        //    get { return (string)base["ormType"]; }
        //    set { base["ormType"] = value; }
        //}

        public override string ToString()
        {
            string output = "SettingElemement   :\n ";
            output += string.Format("AppName   =   {0}\n ", AppName);
            output += string.Format("fileName   =   {0}\n ", FileName);
            //output += string.Format("OrmType   =   {0}\n ", OrmType);
            return output;
        }


        protected override void DeserializeElement(
            XmlReader reader,
             bool serializeCollectionKey)
        {
            base.DeserializeElement(reader,
                serializeCollectionKey);
            // You can your custom processing code here.
        }


        protected override bool SerializeElement(
            XmlWriter writer,
            bool serializeCollectionKey)
        {
            bool ret = base.SerializeElement(writer,
                serializeCollectionKey);
            // You can enter your custom processing code here.
            return ret;

        }


        protected override bool IsModified()
        {
            bool ret = base.IsModified();
            // You can enter your custom processing code here.
            return ret;
        }
    }
}
