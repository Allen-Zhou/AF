using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Configuration;
using System.Collections.Generic;
using TelChina.AF.Sys.Configuration;
using TelChina.AF.Util.Logging;
using System.ServiceModel.Description;
using TelChina.AF.Sys.Serialization;
using System.ServiceModel.Configuration;

namespace TelChina.AF.Sys.Service
{
    public class ServiceProxy
    {
        //const bool isRemoteCall = true;

        private static readonly Dictionary<string, HostSettings> HostDic = GetHosts();
        //private static Dictionary<string, ServiceEndpointElement> _endPointDic;
        private static readonly Dictionary<string, Binding> BindingDic = ServiceConfig.GetBindings();

        private static T GetChannel<T>(Binding binding, EndpointAddress endpointAddress)
        {
            //ToDo config
            //Dictionary<string, HostSettings> appHostDic = new Dictionary<string, HostSettings>();
            // var bindings = ServiceConfig.GetBindings();
            //var endPointDic = ServiceConfig.GetEndpointConfig();
            //Binding servicebinding = GetDefaultBinding(bindings);
            var svSection = AFConfigurationManager.SVGroup;
            //超时时间
            //ToDo 需要使用配置或者从服务定位器获取...
            //binding.ReceiveTimeout = TimeSpan.FromSeconds(20d);
            //binding.CloseTimeout = TimeSpan.FromSeconds(20d);
            //binding.OpenTimeout = TimeSpan.FromSeconds(20d);
            //binding.SendTimeout = TimeSpan.FromSeconds(20d);
            var channelFactory =
                new ChannelFactory<T>(binding, endpointAddress);

            //TODO 需要改为ＡＯＰ方式
            if (typeof(T).FullName == "TelChina.AF.Demo.DemoSV.Interfaces.ICommonCRUDService")
            {
                var op = channelFactory.Endpoint.Contract.Operations.Find("Save");
                var serializerBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (serializerBehavior == null)
                {
                    serializerBehavior = new DataContractSerializerOperationBehavior(op);
                    op.Behaviors.Add(serializerBehavior);
                }
                serializerBehavior.DataContractResolver =
                    new EntityDTODataContractResolver();


                //var saveOpDescription = channelFactory.Endpoint.Contract.Operations.Find("Save");
                //serializerBehavior =
                //saveOpDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                //if (serializerBehavior == null)
                //{
                //    serializerBehavior = new DataContractSerializerOperationBehavior(saveOpDescription);
                //    //serializerBehavior.

                //    saveOpDescription.Behaviors.Add(serializerBehavior);
                //}
                //var answerType = Assembly.Load("TelChina.AF.Demo").GetType("TelChina.AF.Demo.Answer");
                //var answerDTOType = Assembly.Load("TelChina.AF.Demo.DemoSV.Contract").GetType("TelChina.AF.Demo.AnswerDTO");

                //saveOpDescription.KnownTypes.Add(answerType);
                //saveOpDescription.KnownTypes.Add(answerDTOType);

            }
            T bpChannel = channelFactory.CreateChannel();
            return bpChannel;
        }
        internal static T CreateInnerChannel<T>()
        {
            return GetChannel<T>();
        }
        protected static T GetChannel<T>()
        {
            var AppName = typeof(T).FullName.Split('.')[2];
            // var baseAddr = (ConfigurationManager.AppSettings["ServiceBaseUri"]);
            string baseAddr = null;

            if (!HostDic.ContainsKey(AppName))
            {
                if (HostDic.ContainsKey("default"))
                {
                    AppName = "default";
                }
                else
                {
                    //ILogger logger = LogManager.GetLogger("Proxy");
                    //logger.Debug(string.Format("类型{0},对应的应用{1}没有注册,且default配置没有定义,服务无法启动", typeof(T).FullName, AppName));
                    throw new Exception(string.Format("类型{0},对应的应用{1}没有注册,且default配置没有定义,服务无法启动", typeof(T).FullName, AppName));
                }
            }
            baseAddr = string.Format(@"http://{0}:{1}/", HostDic[AppName].HostName, HostDic[AppName].Port);

            //if (string.IsNullOrEmpty(baseAddr))
            //    throw new Exception(string.Format("当前服务{0}所在的应用{1}尚未在Biz.Config中注册", typeof(T).FullName, AppName));

            //var defaultBinding = (ConfigurationManager.AppSettings["DefaultBinding"]);

            Binding servicebinding = ServiceConfig.GetDefaultBinding(AppName, BindingDic);
            //if (isRemoteCall)
            //{
            //var binding = new WSHttpBinding { TransactionFlow = true };
            var result = GetChannel<T>(
                servicebinding,
                new EndpointAddress(baseAddr + typeof(T).FullName));
            return result;
            //}
            //ToDo 添加本地调用方式
            //else
            //{
            //    //本地方式调用
            //    return null;
            //}
        }

        private static Dictionary<string, HostSettings> GetHosts()
        {
            return AFConfigurationManager.SVGroup.Hosts.Cast<HostSettings>().ToDictionary(hostSetting => hostSetting.AppName);
        }

        public static T CreateProxy<T>()
        {
            //var instance = GetChannel<T>();
            var realProxy = new ClientProxy<T>();
            var transparentProxy = (T)realProxy.GetTransparentProxy();
            return transparentProxy;
        }
    }
}
