using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using TelChina.AF.Util.Common;
using System.ServiceModel.Configuration;
using System.ServiceModel;
using TelChina.AF.Util.Logging;

namespace TelChina.AF.Sys.Configuration
{
    public class ServiceConfig
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(() => LogManager.GetLogger("Config"));
        /// <summary>
        /// 平台总体配置节
        /// </summary>
        private static readonly Lazy<System.Configuration.Configuration> BizConfig =
            new Lazy<System.Configuration.Configuration>(ConfigHelper.LoadBindingConfigs);
        /// <summary>
        /// EndPoint配置信息,其中Key是 EndPoint名称,用于匹配特定服务的EndPoint配置
        /// </summary>
        private static readonly Dictionary<string, ServiceEndpointElement> EndpointDic = new Dictionary<string, ServiceEndpointElement>();

        /// <summary>
        /// 从配置中加载的特定 Binding 配置
        /// </summary>
        private static readonly Dictionary<string, Binding> Bindings = new Dictionary<string, Binding>();

        /// <summary>
        /// 获取绑定配置,优先使用App.Config或Web.Config中的配置
        /// </summary>
        /// <returns>key为Binding的Name,Value为Binding实体</returns>
        public static Dictionary<string, Binding> GetBindings()
        {
            //配置只加载一次
            if (Bindings.Count == 0)
            {
                var newBindings = GetBindingsFromCfg(BizConfig.Value);
                Bindings.Merge(newBindings);

                //获取AppConfig中的服务配置信息
                var appCfg = ConfigHelper.LoadAppConfig();
                newBindings = GetBindingsFromCfg(appCfg);
                Bindings.Merge(newBindings);
            }


            return Bindings;
        }

        public static Dictionary<string, ServiceEndpointElement> GetEndpointConfig()
        {
            var appCfg = ConfigHelper.LoadAppConfig();
            var serviceGroupConfig = ServiceModelSectionGroup.GetSectionGroup(appCfg);

            if (serviceGroupConfig != null)
            {
                foreach (ServiceElement serviceConfig in serviceGroupConfig.Services.Services)
                {
                    foreach (ServiceEndpointElement endpointElement in serviceConfig.Endpoints)
                    {
                        if (!Bindings.ContainsKey(endpointElement.BindingConfiguration))
                        {
                            var errorStr = new StringBuilder(1024);
                            //errorStr
                            Logger.Value.Error(string.Format("配置中出现了没有定义的绑定,EndPointName:{0},\t服务契约名:{1},\tBinding配置名{2}",
                                endpointElement.Name, endpointElement.Contract, endpointElement.BindingConfiguration));
                            //bindings.Add(endpointElement.BindingName);
                            continue;
                        }
                        if (!EndpointDic.ContainsKey(endpointElement.Contract))
                            EndpointDic.Add(endpointElement.Contract, endpointElement);
                    }
                }
            }
            return EndpointDic;
        }
        private static Dictionary<string, Binding> GetBindingsFromCfg(System.Configuration.Configuration cfg)
        {
            Dictionary<string, Binding> result = new Dictionary<string, Binding>();

            foreach (var be in BindingsSection.GetSection(cfg).BindingCollections)
            {
                foreach (IBindingConfigurationElement b in be.ConfiguredBindings)
                {
                    var binding = CreateBinding(b, be);
                    result.Add(binding.Name, binding);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据配置创建Binding
        /// </summary>
        /// <param name="bindingConfig"></param>
        /// <param name="be"></param>
        /// <returns></returns>
        private static Binding CreateBinding(IBindingConfigurationElement bindingConfig, BindingCollectionElement be)
        {
            var binding = (Binding)Activator.CreateInstance(be.BindingType);
            bindingConfig.ApplyConfiguration(binding);
            binding.Name = bindingConfig.Name;
            return binding;
        }



        public static Binding GetDefaultBinding(string appName, Dictionary<string, Binding> bindings)
        {
            string defaultBindingName = string.Empty;

            //defaultBindingName = (System.Configuration.ConfigurationManager.AppSettings["DefaultBinding"]);
            var appHostDic = GetAppHosts();
            if (appHostDic.ContainsKey(appName))
            {
                defaultBindingName = appHostDic[appName].DefaultBinding;
            }
            Binding binding = null;

            if (bindings.ContainsKey(defaultBindingName))
            {
                binding = bindings[defaultBindingName];
            }
            else
            {
                binding = new WSHttpBinding
                {
                    TransactionFlow = true,
                    ReceiveTimeout = TimeSpan.FromMinutes(20d),
                    CloseTimeout = TimeSpan.FromSeconds(20d),
                    OpenTimeout = TimeSpan.FromMinutes(20d),
                    SendTimeout = TimeSpan.FromMinutes(20d)
                };
            }
            return binding;
        }

        public static Dictionary<string, HostSettings> GetAppHosts()
        {
            var appHostDic = new Dictionary<string, HostSettings>();
            var svSection = AFConfigurationManager.SVGroup;
            foreach (HostSettings hostSetting in svSection.Hosts)
            {
                if (!appHostDic.ContainsKey(hostSetting.AppName))
                {
                    appHostDic.Add(hostSetting.AppName, hostSetting);
                }
            }
            return appHostDic;
        }
    }

    internal static class DictionaryExtend
    {
        /// <summary>
        /// 字典数据合并,合并后当前集合会包含两个集合的所有成员
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="current"></param>
        /// <param name="source">需要合并的字典,合并后内容不发生变化,对此字典的操作是只读的</param>
        /// <param name="preserveChanges">当此值为true时,如果出现key相同的元素,原始记录会被刷新;为false则会保留当前值</param>
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> current,
            Dictionary<TKey, TValue> source, bool preserveChanges = true)
        {
            if (source == null || source.Count == 0)
            {
                return;
            }
            foreach (var key in source.Keys)
            {
                if (current.ContainsKey(key))
                {
                    if (preserveChanges)
                        current[key] = source[key];
                }
                else
                {
                    current.Add(key, source[key]);
                }
            }
        }
    }
}
