using System;
using System.ServiceModel.Configuration;
using System.Configuration;

namespace TelChina.AF.Service.AOP
{
    public class PolicyInjectionBehaviorElement : BehaviorExtensionElement
    {
        [ConfigurationProperty("policyInjectorName", IsRequired = false, DefaultValue = "")]
        public string PolicyInjectorName
        {
            get
            {
                return this["policyInjectorName"] as string;
            }
            set
            {
                this["policyInjectorName"] = value;
            }
        }

        public override Type BehaviorType
        {
            get { return typeof(PolicyInjectionBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new PolicyInjectionBehavior(this.PolicyInjectorName);
        }
    }
}
