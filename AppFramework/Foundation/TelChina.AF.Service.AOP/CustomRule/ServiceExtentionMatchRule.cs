using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections.Specialized;
using System.Reflection;

namespace TelChina.AF.Service.AOP.CustomRule
{
    [ConfigurationElementType(typeof(CustomMatchingRuleData))]
    public class ServiceExtentionMatchRule : IMatchingRule
    {
        readonly NameValueCollection _attributes = null;
        public ServiceExtentionMatchRule(NameValueCollection attributes)
        {
            this._attributes = attributes;
        }

        public bool Matches(MethodBase member)
        {
            bool result = false;
            if (this._attributes["matchname"] != null)
            {
                if (this._attributes["matchname"] == member.Name)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
