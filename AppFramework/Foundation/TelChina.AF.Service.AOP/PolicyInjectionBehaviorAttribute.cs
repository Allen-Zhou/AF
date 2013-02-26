using System;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace TelChina.AF.Service.AOP
{
    public class PolicyInjectionBehaviorAttribute : Attribute, IContractBehavior
    {
        public string PolicyInjectorName { get; set; }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                        ClientRuntime clientRuntime)
        {
        }
        /// <summary>
        /// 注册 服务实例创建提供者,将基于PIAB的实例生成器注入WCF扩展
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        /// <param name="dispatchRuntime"></param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                          DispatchRuntime dispatchRuntime)
        {
            Type serviceContractType = contractDescription.ContractType;
            dispatchRuntime.InstanceProvider = new PolicyInjectionInstanceProvider(serviceContractType,
                                                                                   this.PolicyInjectorName);
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
    }
}