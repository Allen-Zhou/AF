using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TelChina.AF.Sys.Service;
using TelChina.AF.Sys.Exceptions;
using System.Diagnostics;


namespace TelChina.AF.Demo.DemoSV
{

    public class DemoSVAgent : ServiceProxy
    {
        private readonly IDemoSV _channel;
        private ParamDTO param;
        #region Constuctor

        public DemoSVAgent()
        {
            var result = GetChannel<IDemoSV>();
            _channel = result;
        }

        public ParamDTO Param
        {
            get { return param; }
            set { param = value; }
        }

        /*
                private DemoSVAgent(Binding binding, EndpointAddress endpointAddress)
                {
                    binding.CloseTimeout = TimeSpan.FromMinutes(5d);
                    var bpChannel = GetChannel(binding, endpointAddress);
                    this._channel = bpChannel;
                }
        */


        #endregion

        public ResultDTO Do()
        {
            try
            {
                //var header = new MessageHeader<ProfileContext>();
                //OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("Int32", "System"));

                ResultDTO result = null;
                var contextChannel = _channel as IContextChannel;
                if (contextChannel != null)
                {
                    using (var scope = new OperationContextScope(contextChannel))
                    {
                        var contextManager = new ServiceContextManager();
                        ServiceContextManager.Current = contextManager;
                        result = _channel.Do(Param);
                    }
                }
                else
                {
                    //理论上应该不会走到这里
                    result = _channel.Do(Param);
                    Debug.Assert(true, "服务 Agent里打开的Channel不是 IContextChannel 类型,神马情况?需要确认一下");
                }
                return result;
            }
            catch (FaultException<ExceptionBase> seb)
            {
                //系统异常
                //TO DO Log...
                if (seb.Detail != null)
                    throw seb.Detail;
                else
                    throw new ExceptionBase();
            }
            catch (FaultException<UnhandledException> unex)
            {
                //服务端未处理异常
                //TO DO Log...
                throw unex.Detail;
            }
            catch (Exception e)
            {
                //传输过程中出现的异常
                //TO DO Log...
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                var realChannel = _channel as IDisposable;
                if (realChannel != null)
                {
                    realChannel.Dispose();
                }
            }
        }


    }
}
