using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TelChina.AF.Demo.DemoSV
{
    //回调契约不需要声明服务契约
    [ServiceContract]
    public interface IDualSVCallback
    {
        //[OperationContract]
        [OperationContract(IsOneWay = true)]
        void OnCallBackOneWay(CallBackParamDTO param);
    }
    [DataContract]
    public class CallBackParamDTO
    {
        [DataMember(IsRequired = true)]
        public Guid Token
        {
            get;
            set;
        }
        [DataMember]
        public string Action { get; set; }
        [DataMember(IsRequired = true)]
        public string ClientCallBack { get; set; }
        [DataMember]
        public string Operation { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Message { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(1024);
            result.Append(string.Format("双向通讯服务参数: ClientCallBack:[{0}],Token:[{1}]"
                                        , this.ClientCallBack
                                        , this.Token.ToString()));
            return result.ToString();
        }
    }

    [ServiceContract(CallbackContract = typeof(IDualSVCallback))]
    public interface IDualSV
    {
        [OperationContract]
        void Connect(CallBackParamDTO param);
        [OperationContract]
        void DisConnect(CallBackParamDTO param);
        [OperationContract]
        void Excute(CallBackParamDTO param);
    }
}
