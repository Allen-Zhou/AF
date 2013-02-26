using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TelChina.AF.Demo.DemoSV
{
    /// <summary>
    /// 服务返回值
    /// </summary>
    [DataContract(Namespace = "http://www.telchina.com.cn/RunTimeDemo/V0.1/DTO/2011/06")]
    public class ResultDTO
    {
        [DataMember]
        public string ReturnValue;
    }
}
