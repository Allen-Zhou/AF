using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TelChina.AF.Demo.DemoSV.Interfaces
{
    [ServiceContract]
    public interface IBizDualContract
    {
        [OperationContract]
        void ServiceInvoke(string str);
        [OperationContract(IsOneWay = true)]
        void DoCallBack(string str);
    }
}
