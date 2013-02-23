using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.AF.Sys.Serialization
{

    [AttributeUsageAttribute(AttributeTargets.Class)]
    public class DataContractResolverAttribute : Attribute
    {
        public string TypeFullName { get; set; }

        public string TypeAssembly { get; set; }
    }
}
