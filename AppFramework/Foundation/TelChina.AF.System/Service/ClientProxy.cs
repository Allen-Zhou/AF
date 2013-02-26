using System;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Util.Logging;

//using Model;

namespace TelChina.AF.Sys.Service
{
    public class ClientProxy<T> : RealProxy
    {
        private ILogger logger = LogManager.GetLogger("ServiceClient");
        private T target;

        public ClientProxy()
            : base(typeof(T))
        {
        }
        public override IMessage Invoke(IMessage msg)
        {
            if (object.Equals(target, default(T)))
                target = ServiceProxy.CreateInnerChannel<T>();
            
            using (var contextScope = new OperationContextScope(target as IContextChannel))
            {
                try
                {
                    var contextManager = new ServiceContextManager();
                    ServiceContextManager.Current = contextManager;

                    var callMessage = (IMethodCallMessage)msg;
                    logger.Debug(string.Format("ServiceName:{0},ParamCount:{1}", callMessage.MethodName, callMessage.InArgCount));

                    object returnValue = callMessage.MethodBase.Invoke(target, callMessage.Args);
                    logger.Debug(string.Format("ServiceName:{0},ReturnValue:{1}", callMessage.MethodName, returnValue ?? "NULL"));

                    var result = new ReturnMessage(returnValue, new object[0], 0, null, callMessage);
                    return result;
                }
                catch (FaultException<UnhandledException> unex)
                {
                    //服务端未处理异常
                    //TO DO Log...
                    throw unex.Detail;
                }
                catch (FaultException fe)
                {
                    throw fe.InnerException;
                }
                catch (Exception e)
                {
                    //传输过程中出现的异常
                    logger.Error(e);
                    throw;
                }
                finally
                {
                    var realChannel = target as IDisposable;
                    if (realChannel != null)
                    {
                        realChannel.Dispose();
                    }
                    target = default(T);
                }
            }
        }

    }
}
