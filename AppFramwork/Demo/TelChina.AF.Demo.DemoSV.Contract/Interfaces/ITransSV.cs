using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Service.AOP;
using System.ServiceModel;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Demo.DemoSV.Exceptions;

namespace TelChina.AF.Demo.DemoSV
{
    [ServiceContract(Namespace = "http://www.telchina.com.cn/TRF/V4/Service/2011/03")]
    [PolicyInjectionBehavior]//提供PIAB接口
    public interface ITransSV
    {
        [OperationContract]
        [FaultContract(typeof(ExceptionBase))]
        [FaultContract(typeof(ExpectedException))]
        [FaultContract(typeof(UnhandledException))]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Guid Required(bool isSucceed);

        [OperationContract]
        [FaultContract(typeof(ExceptionBase))]
        [FaultContract(typeof(ExpectedException))]
        [FaultContract(typeof(UnhandledException))]
        [TransactionFlow(TransactionFlowOption.NotAllowed)]
        Guid RequiresNew(bool isSucceed);


        [OperationContract]
        [FaultContract(typeof(ExceptionBase))]
        [FaultContract(typeof(ExpectedException))]
        [FaultContract(typeof(UnhandledException))]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.NotAllowed)]
        Guid NotSupported(bool isSucceed);

        [OperationContract]
        [FaultContract(typeof(ExceptionBase))]
        [FaultContract(typeof(ExpectedException))]
        [FaultContract(typeof(UnhandledException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Supported();

    }
}
