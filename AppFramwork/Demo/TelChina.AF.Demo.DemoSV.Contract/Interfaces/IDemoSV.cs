using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Sys.Service;
using TelChina.AF.Service.AOP;


namespace TelChina.AF.Demo.DemoSV
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ServiceContract(Namespace = "http://www.telchina.com.cn/TRF/V4/Service/2011/03")]
    [PolicyInjectionBehavior]//提供PIAB接口
    public interface IDemoSV
    {
        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="param"></param>
        [OperationContract]
        [FaultContract(typeof(ExceptionBase))]
        [FaultContract(typeof(DemoSVExecption))]
        [FaultContract(typeof(UnhandledException))]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        ResultDTO Do(ParamDTO param);
    }
}
