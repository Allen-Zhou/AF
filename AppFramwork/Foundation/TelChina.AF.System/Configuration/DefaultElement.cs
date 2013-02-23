using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace TelChina.AF.Sys.Configuration
{
    /// <summary>
    /// PL的default配置
    /// 暂时不用
    /// </summary>
    public class PLDefaultElement:ConfigurationElement
    {
        /// <summary>
        /// ormType经讨论已经提出
        /// </summary>
        /// <param name="newAppName"></param>
        /// <param name="newFileName"></param>
        public PLDefaultElement( String newFileName)
        {
            FileName = newFileName;
        }

        public PLDefaultElement()
        {
        }

       /// <summary>
       /// 可以对该属性配置正则表达式来对其进行约束
       /// ConfigurationProperty成员
       /// </summary>
        [ConfigurationProperty("fileName", IsRequired = true)]
        public string FileName
        {
            get { return (string)base["fileName"]; }
            set { base["fileName"] = value; }
        }

        public override string ToString()
        {
            string output = "SettingElemement   :\n ";
            output += string.Format("fileName   =   {0}\n ", FileName);
            return output;
        }

        protected override void DeserializeElement(
            XmlReader reader,
             bool serializeCollectionKey)
        {
            base.DeserializeElement(reader,
                serializeCollectionKey);
        }

        protected override bool SerializeElement(
            XmlWriter writer,
            bool serializeCollectionKey)
        {
            bool ret = base.SerializeElement(writer,
                serializeCollectionKey);
            return ret;
        }

        protected override bool IsModified()
        {
            bool ret = base.IsModified();
            return ret;
        }
    }

    /// <summary>
    /// 服务的default配置
    /// 暂时不用
    /// </summary>
    public class SVDefaultElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty _propDefaultBinding =
            new ConfigurationProperty("defaultBinding", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _propHostName =
            new ConfigurationProperty("hostName", typeof(string), "localhost", ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _propPort =
            new ConfigurationProperty("port", typeof(int), 9999, ConfigurationPropertyOptions.None);

        static SVDefaultElement()
        {
            // Property initialization 
            _properties = new ConfigurationPropertyCollection { _propDefaultBinding, _propHostName, _propPort };
        }

        public SVDefaultElement( string defaultBinding, String hostName = "localhost",
            int port = 9999)
        {
            DefaultBinding = defaultBinding;
            HostName = hostName;
            Port = port;
            //= newFileName;
            //OrmType = ormType;
        }
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }
        // Default constructor, will use default values as defined
        // below.
        public SVDefaultElement()
        {
        }

        [ConfigurationProperty("hostName", IsRequired = true, DefaultValue = "localhost")]
        public string HostName
        {
            get { return (string)base["hostName"]; }
            set { base["hostName"] = value; }
        }
        [ConfigurationProperty("port", IsRequired = true, DefaultValue = 9999)]
        public int Port
        {
            get
            {
                int port = 9999;
                if (Int32.TryParse(base["port"].ToString(), out port))
                    return port;
                else
                {
                    return port;
                }
            }
            set { base["port"] = value; }
        }
        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        public Uri GetHostUri(Binding binding)
        {
            //ToDo  如果传入binding 为空应使用默认binding来确定Scheme
            //ToDo  主机不是在本地应该如何处理?
            string scheme = "http";

            if (binding != null)
            {
                scheme = binding.Scheme;
            }
            var result = new Uri(string.Format(@"{0}://{1}:{2}/", scheme, HostName, Port));

            return result;
        }
        [ConfigurationProperty("defaultBinding", IsRequired = true)]
        public string DefaultBinding
        {
            get { return (string)base["defaultBinding"]; }
            set { base["defaultBinding"] = value; }
        }

        public override string ToString()
        {
            string output = "SVHostSettingElemement:\n ";
            output += string.Format("DefaultBinding   =   {0}\n ", DefaultBinding);
            output += string.Format("HostName   =   {0}\n ", HostName);
            output += string.Format("Port   =   {0}\n ", Port);
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