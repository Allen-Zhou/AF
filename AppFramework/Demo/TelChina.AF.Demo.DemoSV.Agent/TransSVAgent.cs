using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TelChina.AF.Sys.Service;
using TelChina.AF.Sys.Exceptions;
using System.Diagnostics;


namespace TelChina.AF.Demo.DemoSV
{

    public class TransSVAgent : ServiceProxy
    {
        private readonly ITransSV _channel;

        #region Constuctor

        public TransSVAgent()
        {
            var result = GetChannel<ITransSV>();
            _channel = result;
        }

        public void Close()
        {
            var channel = this._channel as IDisposable;
            if (channel != null)
            {
                channel.Dispose();
            }
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

        public Guid Required(bool isSucceed)
        {
            try
            {
                //var header = new MessageHeader<ProfileContext>();
                //OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("Int32", "System"));
                Guid result = Guid.Empty;

                var contextChannel = _channel as IContextChannel;
                if (contextChannel != null)
                {
                    using (var scope = new OperationContextScope(contextChannel))
                    {
                        var contextManager = new ServiceContextManager();
                        ServiceContextManager.Current = contextManager;
                        result = _channel.Required(isSucceed);
                    }
                }
                else
                {
                    //WCF调用方式 理论上应该不会走到这里,在本地调用方式进行处理
                    result = _channel.Required(isSucceed);
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
            catch (FaultException fe)
            {
                throw fe.InnerException;
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

        public Guid RequiresNew(bool isSucceed)
        {
            try
            {
                //var header = new MessageHeader<ProfileContext>();
                //OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("Int32", "System"));
                Guid result = Guid.Empty;

                var contextChannel = _channel as IContextChannel;
                if (contextChannel != null)
                {
                    using (var scope = new OperationContextScope(contextChannel))
                    {
                        var contextManager = new ServiceContextManager();
                        ServiceContextManager.Current = contextManager;
                        result = _channel.RequiresNew(isSucceed);
                    }
                }
                else
                {
                    //理论上应该不会走到这里
                    result = _channel.Required(isSucceed);
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

        public Guid NotSupported(bool isSucceed)
        {
            try
            {
                //var header = new MessageHeader<ProfileContext>();
                //OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("Int32", "System"));
                Guid result = Guid.Empty;

                var contextChannel = _channel as IContextChannel;
                if (contextChannel != null)
                {
                    using (var scope = new OperationContextScope(contextChannel))
                    {
                        var contextManager = new ServiceContextManager();
                        ServiceContextManager.Current = contextManager;
                        result = _channel.NotSupported(isSucceed);
                    }
                }
                else
                {
                    //理论上应该不会走到这里
                    result = _channel.NotSupported(isSucceed);
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

        public void Supported(bool isSucceed)
        {
            try
            {
                //var header = new MessageHeader<ProfileContext>();
                //OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("Int32", "System"));
                //Guid result = Guid.Empty;

                var contextChannel = _channel as IContextChannel;
                if (contextChannel != null)
                {
                    using (var scope = new OperationContextScope(contextChannel))
                    {
                        var contextManager = new ServiceContextManager();
                        ServiceContextManager.Current = contextManager;
                        _channel.Supported();
                    }
                }
                else
                {
                    //WCF调用方式 理论上应该不会走到这里,在本地调用方式进行处理
                    _channel.Supported();
                }
                //return result;
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
            catch (FaultException fe)
            {
                throw fe.InnerException;
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
