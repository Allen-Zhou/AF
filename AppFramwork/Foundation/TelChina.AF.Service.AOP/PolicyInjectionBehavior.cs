using System;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace TelChina.AF.Service.AOP
{
    public class PolicyInjectionBehavior : IEndpointBehavior
    {
        private string _policyInjectorName;
        public PolicyInjectionBehavior(string policyInjectorName)
        {
            this._policyInjectorName = policyInjectorName;
        }
         public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters){ }
         public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime){ }
         public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
         {
             Type serviceContractType = endpoint.Contract.ContractType;
             endpointDispatcher.DispatchRuntime.InstanceProvider = new PolicyInjectionInstanceProvider(serviceContractType, this._policyInjectorName);
         }
         public void Validate(ServiceEndpoint endpoint){ }
     }
 } 