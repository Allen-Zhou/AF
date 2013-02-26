using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.IO;
using System.Reflection;
using System.ServiceModel.Description;
using TelChina.AF.Service.AOP;
using TelChina.AF.Util.Common;
using System.ServiceModel.Configuration;
using TelChina.AF.Util.Logging;
using System.Configuration;
using System.ServiceModel.Channels;
using TelChina.AF.Sys.Configuration;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Sys.Serialization;
//using System.ServiceModel.Channels;

namespace TelChina.AF.Service.AppHosting
{
    public class AppHost
    {
        private static List<ServiceHost> SvList = new List<ServiceHost>();
        /// <summary>
        /// 启动服务引擎
        /// </summary>
        public static void Start()
        {

            AppDomain.CurrentDomain.ProcessExit += EndProcess;
            try
            {
                AFConfigurationManager.Setup();
                var bpList = GetSVs();
                StartHost(bpList);
                bpList.Clear();
            }
            catch (Exception ex)
            {
                HostingLoggerHelper.Error(ex);
                DealException(ex);
            }
            //Console.ReadLine();
        }

        public static void Stop()
        {
            if (SvList != null && SvList.Count > 0)
            {

                SvList.ForEach(sv =>
                                   {
                                       try
                                       {
                                           sv.Close();
                                       }
                                       catch (Exception ex)
                                       {
                                           HostingLoggerHelper.Error(ex);
                                       }

                                   });
                SvList.Clear();
            }

            GC.Collect();
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="ex"></param>
        private static void DealException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                DealException(ex.InnerException);
            }
            Console.WriteLine(string.Format("异常类型{0},\n消息{1},\n堆栈{2}",
                ex.GetType(), ex.Message, ex.StackTrace));
        }

        /// <summary>
        /// 启动ServiceHost
        /// </summary>
        /// <param name="bpList"></param>
        private static void StartHost(IEnumerable<Type> bpList)
        {
            var baseAddr = (System.Configuration.ConfigurationManager.AppSettings["ServiceBaseUri"]);
            //var endPointDic = new Dictionary<string, ServiceEndpointElement>();


            //var bindingConfig = ConfigHelper.LoadBindingConfigs();
            //var bindings = new Dictionary<string, Binding>();
            //foreach (var be in BindingsSection.GetSection(bindingConfig).BindingCollections)
            //{
            //    foreach (IBindingConfigurationElement b in be.ConfiguredBindings)
            //    {
            //        var binding = CreateBinding(b, be);
            //        bindings.Add(binding.Name, binding);
            //    }
            //}

            var bindings = ServiceConfig.GetBindings();
            var endPointDic = ServiceConfig.GetEndpointConfig();
            Dictionary<string, HostSettings> appHostDic = ServiceConfig.GetAppHosts();



            //var totalCfg = ServiceModelSectionGroup.GetSectionGroup(System.Configuration.ConfigurationManager.OpenExeConfiguration(""));// exe.config 或者web.config
            //if (totalCfg != null)
            //{
            //    foreach (ServiceElement serviceConfig in totalCfg.Services.Services)
            //    {
            //        foreach (ServiceEndpointElement endpointElement in serviceConfig.Endpoints)
            //        {
            //            if (!bindings.ContainsKey(endpointElement.BindingName))
            //            {
            //                //bindings.Add(endpointElement.BindingName);
            //            }
            //            endPointDic.Add(endpointElement.Contract, endpointElement);
            //        }
            //    }
            //}
            foreach (Type bpType in bpList)
            {

                var serviceContract = (from i in bpType.GetInterfaces()
                                       where i.GetCustomAttributes(typeof(ServiceContractAttribute), false).Any()
                                       select i).FirstOrDefault();
                if (serviceContract == null)
                {
                    continue;
                }
                //var host = new ServiceHost(bpType, new Uri(baseAddr)) { CloseTimeout = TimeSpan.FromMinutes(20d) };
                ServiceHost host = null;
                var AppName = GetAppName(bpType);
                if (string.IsNullOrEmpty(AppName))
                {
                    continue;
                }
                if (!appHostDic.ContainsKey(AppName))
                {
                    if (!appHostDic.ContainsKey("default"))
                    {
                        HostingLoggerHelper.Debug(string.Format("类型{0} 对应的应用{1} 没有注册,且默认服务没有启用,无法注册服务",
                                                                bpType.AssemblyQualifiedName, AppName));
                        continue;
                    }
                    else
                    {
                        //Modifiedby zhoulun 如果没有注册对应的应用，需要使用Default作为应用名称
                        AppName = "default";
                    }
                }



                if (endPointDic.ContainsKey(serviceContract.FullName))
                {
                    if (serviceContract.FullName != null)
                    {
                        var cfg = endPointDic[serviceContract.FullName];

                        host = new ServiceHost(bpType, cfg.Address);
                        host.AddServiceEndpoint(serviceContract, bindings[cfg.BindingConfiguration], "");
                        HostingLoggerHelper.Debug(string.Format("服务{0},使用本地配置初始化:Addr:[{1}],\tBindingName[{2}],\tBindingType[{3}]"
                            , serviceContract.FullName, cfg.Address, cfg.BindingName, cfg.BindingConfiguration));
                    }
                }
                else
                {

                    Binding servicebinding = ServiceConfig.GetDefaultBinding(AppName, bindings);
                    var addr = new Uri(appHostDic[AppName].GetHostUri(servicebinding) + serviceContract.FullName);
                    //var addr = new Uri(baseAddr + serviceContract.FullName);
                    host = new ServiceHost(bpType, addr) { CloseTimeout = TimeSpan.FromMinutes(20d) };
                    host.AddServiceEndpoint(serviceContract, servicebinding, "");
                    HostingLoggerHelper.Debug(string.Format("服务{0},使用默认配置初始化:Addr:[{1}],\tBindingName[{2}]"
                          , serviceContract.FullName, addr.ToString(), servicebinding.Name));
                }

                //ContractDescription cd = new ContractDescription(bpType.FullName);

                //ServiceEndpoint endpoint = new ServiceEndpoint();
                //endpoint.Binding = wsBinding;
                //endpoint.Address = new EndpointAddress(serviceContract.FullName);
                //endpoint.Behaviors.Add(new ServiceDebugBehavior());


                //host.AddServiceEndpoint()
                //host.AddServiceEndpoint(endpoint);      

                #region 不加标签序列化解环
                // Find the ContractDescription of the operation to find.
                ContractDescription cd = host.Description.Endpoints[0].Contract;
                // 将所有的Behavior中的DataContractSerializerOperationBehavior全部找出来做替换
                foreach (var operation in cd.Operations)
                {
                    // Find the serializer behavior
                    var serializerBehavior = operation.Behaviors.
                       Find<DataContractSerializerOperationBehavior>();
                    if (serializerBehavior != null)
                    {
                        operation.Behaviors.Remove(serializerBehavior);
                    }
                    // 添加自己写的序列化方法
                    operation.Behaviors.Add(new DataContractSerializerOperationBehaviorEx(
                        operation) { MaxItemsInObjectGraph = 65536, IgnoreExtensionDataObject = true }
                        );
                }
                #endregion


                #region 通用CRUD 处理DataContractTypeResover
                if (bpType.FullName == "TelChina.AF.Demo.DemoSV.CommonCRUDService")
                {
                    foreach (var opDescription in cd.Operations)
                    {
                        var serializerBehavior =
                            opDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                        if (serializerBehavior == null)
                        {
                            serializerBehavior = new DataContractSerializerOperationBehavior(opDescription);
                            opDescription.Behaviors.Add(serializerBehavior);
                        }
                        serializerBehavior.DataContractResolver =
                            new EntityDTODataContractResolver();
                    }

                    //var saveOpDescription = cd.Operations.Find("Save");
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

                #endregion

                EnableMexGet(host);

                host.Open();
                SvList.Add(host);
                HostingLoggerHelper.Debug(string.Format("服务{0}已启动,服务地址为:{1}", serviceContract.Name, host.BaseAddresses.FirstOrDefault().AbsoluteUri));
            }
        }



        private static string GetAppName(Type svType)
        {
            var strnNameSpace = svType.Namespace;
            if (!string.IsNullOrEmpty(strnNameSpace))
            {
                var names = strnNameSpace.Split('.');
                if (names.Length >= 3)
                    return names[2];
            }
            HostingLoggerHelper.Debug(string.Format("类型{0} 的名称空间不符合规范", svType.AssemblyQualifiedName));
            return string.Empty;
        }
        private static Binding CreateBinding(IBindingConfigurationElement b, BindingCollectionElement be)
        {
            var binding = (Binding)Activator.CreateInstance(be.BindingType);
            b.ApplyConfiguration(binding);
            binding.Name = b.Name;
            return binding;
        }

        private static Binding GetDefaultBinding(Dictionary<string, Binding> bindings)
        {
            Binding binding = null;
            var defaultBindingName = (System.Configuration.ConfigurationManager.AppSettings["DefaultBinding"]);
            if (bindings.ContainsKey(defaultBindingName))
            {
                binding = bindings[defaultBindingName];
            }
            else
            {
                binding = new WSHttpBinding
                {
                    TransactionFlow = true,
                    ReceiveTimeout = TimeSpan.FromHours(20d),
                    CloseTimeout = TimeSpan.FromSeconds(20d),
                    OpenTimeout = TimeSpan.FromHours(20d),
                    SendTimeout = TimeSpan.FromHours(20d)
                };
            }
            return binding;
        }

        /// <summary>
        /// 启动元数据获取行为
        /// </summary>
        /// <param name="host"></param>
        private static void EnableMexGet(ServiceHost host)
        {
            var mdBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (mdBehavior == null)
            {
                mdBehavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
                host.Description.Behaviors.Add(mdBehavior);
                host.AddServiceEndpoint(
                    ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexHttpBinding(),
                    "mex");
            }
        }

        /// <summary>
        /// 查找服务
        /// </summary>
        /// <returns></returns>
        private static List<Type> GetSVs()
        {
            var result = new List<Type>();
            var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
            //找不到Lib目录就在当前路径下找
            if (!Directory.Exists(baseDirectory))
            {
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            var libFiles = Directory.GetFiles(baseDirectory, "*.Impl.dll");

            HostingLoggerHelper.Debug(string.Format("搜索服务实现的路径为:{0} 找到符合条件的dll个数为{1}", baseDirectory, libFiles.Length));
            foreach (var libFile in libFiles)
            {
                var lib = Assembly.LoadFile(libFile);
                AppDomain.CurrentDomain.Load(lib.FullName);
                var svType = (from t in lib.GetTypes()
                              where
                                  (from i in t.GetInterfaces()
                                   where i.GetCustomAttributes(typeof(ServiceContractAttribute), false).Any()
                                   select i).Any()
                              select t).ToList();
                if (svType.Count > 0)
                {
                    result.AddRange(svType);
                }
            }
            return result;
        }

        /// <summary>
        /// 进程终止时释放非托管资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void EndProcess(object sender, EventArgs e)
        {
            Console.WriteLine("Is Ending.Press any key to continue...");
            Console.Read();
        }
    }

}
