using System;   
using System.Collections.Generic;
using System.Linq;
using TelChina.AF.Sys.AOP;
using System.Transactions;
using TelChina.AF.Util.Logging;
using TelChina.AF.Sys.Exceptions;
using System.ServiceModel;
using TelChina.AF.Sys.Context;

namespace TelChina.AF.Sys.Service
{
    /// <summary>
    /// 服务实现的基类,实现AOP引擎
    /// </summary>
    public abstract class ServiceBase : MarshalByRefObject
    {

        protected ILogger Logger;
        protected ServiceBase()
        {
            Logger = LogManager.GetLogger("Service" + GetType().Name);
        }
        /// <summary>
        /// 当前用户登录信息上下文
        /// </summary>
        protected ServiceContext CurrentContext;

        /// <summary>
        /// 执行服务逻辑的包装方法,包含了AOP执行逻辑
        /// </summary>
        /// <returns></returns>
        protected object Excute(StrategyBase opObj)
        {
            var doMethod = opObj.GetType().GetMethod("Do");
            var aopFrames = doMethod.GetCustomAttributes(typeof(AOPAttribute), true)
                .Where(f => (f as AOPAttribute) != null)
                .Select(f => f as AOPAttribute);

            Stack<AOPAttribute> frameStack = null;
            if (aopFrames.Any())
            {
                frameStack = new Stack<AOPAttribute>(aopFrames.Count());
                foreach (AOPAttribute frame in aopFrames)
                {
                    frame.BeforeInvoke(this);
                    frameStack.Push(frame);
                }
            }

            var result = opObj.Do();// return value...

            if (frameStack != null)
            {
                while (frameStack.Count > 0)
                {
                    var frame = frameStack.Pop();
                    frame.AfterInvoke(this);
                }
            }

            return result;
        }

        /// <summary>
        /// 初始化上下文
        /// </summary>
        /// <param name="currentContext"></param>
        protected void InitThreadContext(ServiceContext currentContext)
        {
            CurrentContext = currentContext;
        }
        protected void DealTransaction()
        {
            if (Transaction.Current != null)
            {
                Logger.Debug("[服务端进入事务处理模式]:");
                LogTransactionInfo(Transaction.Current);
                Transaction.Current.TransactionCompleted +=
                    (obj, args) =>
                        {
                            Logger.Debug("[服务端进入事务处理完成]:");
                            LogTransactionInfo(args.Transaction);
                        };
            }
            else
            {
                Logger.Debug("[服务处于无事务模式]:");
            }
        }
        private void LogTransactionInfo(Transaction trans)
        {
            Logger.Debug(string.Format("事务属性:\n事务状态:{0}\n创建时间{1}\n分布式标识{2}\n本地标识{3}",
                trans.TransactionInformation.Status,
                trans.TransactionInformation.CreationTime,
                trans.TransactionInformation.DistributedIdentifier,
                trans.TransactionInformation.LocalIdentifier));
        }

        /// <summary>
        /// 执行服务的实现方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceImpl"></param>
        /// <returns></returns>
        protected T ServiceInvoke<T>(Func<T> serviceImpl)
        {
            try
            {
                Logger.Debug(string.Format("服务{0}开始执行", GetType().FullName));
                //bool isTransRoot = false;
                ////如果没有外层事务,则创建并提交事务
                ////否则继承外层事务,且不提交
                //if (Transaction.Current == null || Transaction.Current.TransactionInformation.DistributedIdentifier == Guid.Empty)
                //{
                //    isTransRoot = true;
                //}
                //using (var trans = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(1, 1, 1, 0)))
                //{
                DealTransaction();
                if (ServiceContextManager.Current != null)
                {
                    //传递上下文
                    Logger.Debug(string.Format("<=Message Header 传输内容:{0}  =>",
                                                     ServiceContextManager.Current));
                    CurrentContext = ServiceContextManager.Current.Value;
                    ContextSession.Current = CurrentContext;
                }
                //var header = OperationContext.Current.IncomingMessageHeaders.GetHeader<int>("Int32", "System");
                //Console.WriteLine(string.Format("<=Message Header 传输:{0}  =>", header));

                #region 创建和初始化操作对象

                //var opObj = new DemoSVImplement { Param = param };

                #endregion

                //InitThreadContext(currentContext);
                DealTransaction();
                var result = serviceImpl();

                //内部的 事务资源一定要提交,否则服务会抛出事务失败的异常
                //此处提交只是提交当前事务源,还需要协调者统一处理才能确定事务的最终状态,
                //如果外部没有提交的话可以回滚
                //trans.Complete();
                //var t = (Transaction.Current as CommittableTransaction);
                //if (t != null)
                //{
                //    t.Commit();
                //}
                Logger.Debug(string.Format("服务{0}执行结束", GetType().FullName));
                return result;
                //}
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
            //return Guid.Empty;
        }

        /// <summary>
        /// 执行服务的实现方法
        /// </summary>
        /// <param name="serviceImpl"></param>
        protected void ServiceInvoke(Action serviceImpl)
        {
            ServiceInvoke<object>(() =>
                                      {
                                          serviceImpl();
                                          return null;
                                      }
                );
        }
    }
}
