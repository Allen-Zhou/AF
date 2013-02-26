using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TelChina.AF.Demo.DemoSV
{
    /// <summary>
    /// 服务参数
    /// </summary>   
    [DataContract(Namespace = "http://www.telchina.com.cn/RunTimeDemo/V0.1/DTO/2011/06")]
    public class ParamDTO
    {
        [DataMember]
        public string ParamName;
        [DataMember]
        public string Value;
        /// <summary>
        /// 是否调用成功,如果为否直接抛出SV异常,用于单元测试
        /// </summary>
        [DataMember]
        public bool IsSucceed;
    }
    [DataContract]
    public class DerivedParamDTO : ParamDTO
    {
        [DataMember]
        public string ExtendField;
    }
}
