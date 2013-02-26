using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.ServiceModel.Description;

namespace TelChina.AF.Sys.Serialization
{
    /// <summary>
    /// 这个标签只能用在服务端，客户端怎么处理？
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TypeResolverBehaviorAttribute :
        Attribute, IServiceBehavior
    {

        void IServiceBehavior.Validate
            (ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {

        }



        public void AddBindingParameters
            (ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
             BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior
            (ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
