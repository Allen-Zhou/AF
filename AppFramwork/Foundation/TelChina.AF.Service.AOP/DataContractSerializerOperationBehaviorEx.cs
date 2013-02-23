using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Text;

namespace TelChina.AF.Service.AOP
{
    public class DataContractSerializerOperationBehaviorEx : DataContractSerializerOperationBehavior
    {
        public DataContractSerializerOperationBehaviorEx(OperationDescription operation)
            : base(operation)
        {

        }

        public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
        {
            return new DataContractSerializer(type, name, ns, knownTypes, this.MaxItemsInObjectGraph, this.IgnoreExtensionDataObject, true, this.DataContractSurrogate, this.DataContractResolver);
        }

        public override XmlObjectSerializer CreateSerializer(Type type, System.Xml.XmlDictionaryString name, System.Xml.XmlDictionaryString ns, IList<Type> knownTypes)
        {
            return new DataContractSerializer(type, name, ns, knownTypes, this.MaxItemsInObjectGraph, this.IgnoreExtensionDataObject, true, this.DataContractSurrogate, this.DataContractResolver);
        }
    }
}
