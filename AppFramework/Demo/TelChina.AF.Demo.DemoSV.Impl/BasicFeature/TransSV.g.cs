using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
//using TelChina.AF.Demo.DomainModel.Model;
using TelChina.AF.Util.Common;
using System.Transactions;
using TelChina.AF.Service.AOP.AOPAttribute;
using System.Threading;
using System.Data.Objects;
//using TelChina.AF.Domain.Core;
using TelChina.AF.Sys.Service;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Util.Logging;

namespace TelChina.AF.Demo.DemoSV
{
    [ServiceBehavior(
        //异常传播
    IncludeExceptionDetailInFaults = DebugHelper.IncludeExceptionDetailInFaults,
        //事务隔离级别
    TransactionIsolationLevel = IsolationLevel.Serializable)]

    public partial class TransSV : ServiceBase, ITransSV
    {
        public TransSV()
        {

        }
        [LogAttribute]
        //[TransactionHandlerAttribute(Option = TransactinOption.Required)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public Guid Required(bool isSucceed)
        {
            return base.ServiceInvoke(() => DoExtend(isSucceed));
        }

        [LogAttribute]
        [OperationBehavior(TransactionScopeRequired = true)]
        public Guid RequiresNew(bool isSucceed)
        {
            try
            {
                //如果没有外层事务,则创建并提交事务
                //否则继承外层事务,且不提交
                //if (Transaction.Current == null || Transaction.Current.TransactionInformation.DistributedIdentifier == Guid.Empty)
                //{
                //    isTransRoot = true;
                //}
                using (var trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(1, 1, 1, 0)))
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

                    //var opObj = new DemoSVImplement { Param = param };

                    #endregion

                    base.InitThreadContext(CurrentContext);
                    DealTransaction();
                    var result = DoExtend(isSucceed);

                    trans.Complete();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
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
        }

        [LogAttribute]
        [OperationBehavior(TransactionScopeRequired = false)]
        public Guid NotSupported(bool isSucceed)
        {
            try
            {
                //如果没有外层事务,则创建并提交事务
                //否则继承外层事务,且不提交
                //if (Transaction.Current == null || Transaction.Current.TransactionInformation.DistributedIdentifier == Guid.Empty)
                //{
                //    isTransRoot = true;
                //}
                using (var trans = new TransactionScope(TransactionScopeOption.Suppress, new TimeSpan(1, 1, 1, 0)))
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

                    //var opObj = new DemoSVImplement { Param = param };

                    #endregion

                    base.InitThreadContext(CurrentContext);
                    DealTransaction();
                    var result = DoExtend(isSucceed);

                    trans.Complete();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
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
        }

        [LogAttribute]
        [OperationBehavior(TransactionScopeRequired = false)]
        public void Supported()
        {
            base.ServiceInvoke(() =>
                                          {
                                              Logger.Debug("执行服务");
                                              return 0;
                                          }
                                          );
        }
    }
}
