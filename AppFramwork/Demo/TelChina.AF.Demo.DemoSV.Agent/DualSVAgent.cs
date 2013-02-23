using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using TelChina.AF.Demo.DemoSV;
using System.ServiceModel;
using TelChina.AF.Util.Logging;
using TelChina.AF.Demo.DemoSV.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace TelChina.AF.Demo.DemoSV
{
    //基本的回调测试，已经废弃
    public class DualSVAgent : IDualSVCallback, IDisposable
    {
        ILogger logger = LogManager.GetLogger("DualSVClient");
        private DualSVClient _proxy;
        public void Dispose()
        {
            logger.Debug("Agent Disposing...");
            _proxy.Close();
            _proxy = null;
            logger.Debug("Agent Disposed");
        }

        public DualSVAgent()
        {
            var context = new InstanceContext(this);
            _proxy = new DualSVClient(context);
        }

        public void Do()
        {
            logger.Debug("Excute Operation...");
            _proxy.Excute(new CallBackParamDTO());
            logger.Debug("Operation Excuted");

        }
        public void OnCallBackOneWay(CallBackParamDTO param)
        {
            logger.Debug("Doing Callback at agent...");
        }
    }

    public class DualSVAgent<T>
    {
        private DualSVClient _client;
        private Guid Token;
        public DualSVAgent()
        {
            //ToDo 需要成通过T接口创建客户端实例的代码而不是写死
            var callbackImpl = new CallBack<T>();
            var context = new InstanceContext(callbackImpl);

            _client = new DualSVClient(context);
            this.Token = Guid.NewGuid();
        }

        public void Connect()
        {
            var param = GetParam();
            _client.Connect(param);
        }
        public void Excute(string action, string message)
        {
            var param = GetParam();
            param.Action = action;
            param.Message = message;
            _client.Excute(param);
        }
        private CallBackParamDTO GetParam()
        {
            var param = new CallBackParamDTO();
            param.ClientCallBack = string.Format("{0},{1}", typeof(T).FullName, typeof(T).Assembly.FullName);
            param.Token = this.Token;
            param.Address = string.Format("LocalAddr:[{0}],RemoteAddr[{1}]",
                                          _client.InnerChannel.LocalAddress, _client.InnerChannel.RemoteAddress);
            return param;
        }

        public void DisConnect()
        {
            var param = GetParam();
            _client.DisConnect(param);
            _client.Close();
        }


    }

    public class CallBack<T> : IDualSVCallback
    {
        private T bizCallBack;
      
        public CallBack()
        {
            //ToDo GetExecutingAssembly 这个方法可能不对
            var assembly = Assembly.GetExecutingAssembly();
            var type = (from t in assembly.GetTypes()
                        where t.GetInterface(typeof(T).FullName) != null
                        select t).FirstOrDefault();
            if (type != null)
            {
                var callback = assembly.CreateInstance(type.FullName);
                if (callback is T)
                    this.bizCallBack = (T)callback;
            }
            //this.bizCallBack = bizCallBack;
        }

        public CallBack(T bizCallBack)
        {
            this.bizCallBack = bizCallBack;
        }

        readonly ILogger logger = LogManager.GetLogger("DualSVClient");
        public void OnCallBackOneWay(CallBackParamDTO param)
        {


            if (string.IsNullOrEmpty(param.Action))
            {
                logger.Debug("回调参数中没有提供ActionName");
                return;
            }

            var action = typeof(T).GetMethod(param.Action);

            if (action != null)
            {
                action.Invoke(this.bizCallBack, new object[] { param.Message });
            }
        }
    }

    public class BizCallBack: IBizDualContract
    {
        private DualSVAgent<IBizDualContract> _agent;

        public BizCallBack(DualSVAgent<IBizDualContract> agent)
        {
            this._agent = agent;
        }

        ILogger logger = LogManager.GetLogger("BizCallBack");
        public void ServiceInvoke(string str)
        {
            _agent.Excute("ServiceInvoke", str);
        }

        public void DoCallBack(string str)
        {
            logger.Debug(string.Format("Doing Callback at agent...param is{0}", str));
        }
    }
    public class DualSVClient : DuplexClientBase<IDualSV>, IDualSV
    {
        #region Constructor


        public DualSVClient(object callbackInstance)
            : base(callbackInstance)
        {
        }

        public DualSVClient(object callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
        }

        public DualSVClient(object callbackInstance, string endpointConfigurationName, string remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public DualSVClient(object callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public DualSVClient(object callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
        }

        public DualSVClient(object callbackInstance, ServiceEndpoint endpoint)
            : base(callbackInstance, endpoint)
        {
        }

        public DualSVClient(InstanceContext callbackInstance)
            : base(callbackInstance)
        {
        }

        public DualSVClient(InstanceContext callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
        }

        public DualSVClient(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public DualSVClient(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public DualSVClient(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
        }

        public DualSVClient(InstanceContext callbackInstance, ServiceEndpoint endpoint)
            : base(callbackInstance, endpoint)
        {
        }

        #endregion Constructor

        public void Connect(CallBackParamDTO param)
        {
            Channel.Connect(param);
        }

        public void DisConnect(CallBackParamDTO param)
        {
            Channel.DisConnect(param);
        }

        public void Excute(CallBackParamDTO param)
        {
            Channel.Excute(param);
        }
    }


}
