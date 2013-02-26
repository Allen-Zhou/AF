using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;
using TelChina.AF.Util.Common;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Sys.Service;
using TelChina.AF.Service.AOP.AOPAttribute;
//using TelChina.AF.System.AOP;

namespace TelChina.AF.Demo.DemoSV
{
    /// <summary>
    /// 服务入口类
    /// </summary>
    [ServiceBehavior(
        //异常传播
        IncludeExceptionDetailInFaults = DebugHelper.IncludeExceptionDetailInFaults,
        //事务隔离级别
        TransactionIsolationLevel = IsolationLevel.Serializable)]
    public partial class DemoSV : ServiceBase, IDemoSV
    {
        #region ISV Members
        /// <summary>
        /// 服务入口
        /// </summary>
        /// <param name="param">服务模型中定义的参数</param>
        /// <returns>服务模型中定义的返回值</returns>
        [OperationBehavior(TransactionScopeRequired = true)]
        public ResultDTO Do(ParamDTO param)
        {
            ResultDTO result = null;
            try
            {
                if (ServiceContextManager.Current != null)
                {
                    //传递上下文
                    Console.WriteLine(string.Format("<=Message Header 传输内容:{0}  =>",
                        ServiceContextManager.Current));
                    //this.currentContext = ServiceContextManager.Current.Value;
                   
                }
                //var header = OperationContext.Current.IncomingMessageHeaders.GetHeader<int>("Int32", "System");
                //Console.WriteLine(string.Format("<=Message Header 传输:{0}  =>", header));

                #region 创建和初始化操作对象
                var opObj = new DemoSVImplement { Param = param };
                #endregion
                base.InitThreadContext(CurrentContext);
                DealTransaction();
                //result = base.Excute(opObj) as ResultDTO;
                opObj.Do_Ex();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionBase)
                {
                    throw new FaultException<ExceptionBase>(new ExceptionBase(ex), new FaultReason("业务异常"));
                }
                throw new FaultException<UnhandledException>(new UnhandledException(ex), new FaultReason("未知异常"));
            }
            finally
            {
                //做一些清理工作
            }
            return result;
        }

      
        #endregion

        //public override object Do()
        //{
        //    DemoSVImplement implObj = new DemoSVImplement();
        //    //implObj.Param = param;
        //    return null;
        //}
    }

    /// <summary>
    /// 服务业务实现
    /// </summary>
    internal partial class DemoSVImplement : StrategyBase
    {
        /// <summary>
        /// 服务参数
        /// </summary>
        public ParamDTO Param { get; set; }

        [LogAttribute]
        public override object Do()
        {
            return this.Do_Ex();
        }
    }

}
