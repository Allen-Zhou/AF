using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.Service;
using TelChina.AF.Demo.DemoSV.Interfaces;
using System.ServiceModel;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Sys.DTO;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.DemoSV
{
    public class CommonCRUDSvAgent : ServiceProxy
    {
        private readonly ICommonCRUDService _channel;
        #region Constuctor

        public CommonCRUDSvAgent()
        {
            var result = GetChannel<ICommonCRUDService>();
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

        public void Add(DTOBase entityDTO)
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
                        _channel.Add(new List<object>() { entityDTO });
                    }
                }
                else
                {
                    //WCF调用方式 理论上应该不会走到这里,在本地调用方式进行处理

                }

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
        #endregion

    }
}
