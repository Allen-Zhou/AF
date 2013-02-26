using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Demo.DemoSV;
using System.ServiceModel;
using TelChina.AF.Util.Logging;
using System.Reflection;

namespace TelChina.AF.Demo.DemoSV
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        IncludeExceptionDetailInFaults = true,
        ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class DualSV : IDualSV
    {
        //[ThreadStatic]
        private static ILogger logger = LogManager.GetLogger("DualSV");

        /// <summary>
        /// 通道缓存,外层Dic Key为ActionName,内层Dic的key是已经注册的通道的Guid,value是通道对象
        /// </summary>
        private static Dictionary<string, Dictionary<Guid, object>> _callbackChannel;

        static DualSV()
        {
            _callbackChannel = new Dictionary<string, Dictionary<Guid, object>>();
        }
        /// <summary>
        /// 建立双向通讯链接
        /// </summary>
        /// <param name="param"></param>
        public void Connect(CallBackParamDTO param)
        {

            if (param == null)
            {
                logger.Error("参数为Null,直接返回,无法建立连接");
                return;
            }

            logger.Debug(string.Format("双向服务建立连接参数信息:{0}", param.ToString()));

            //var callBack =
            //  OperationContext.Current.GetCallbackChannel<IDualSVCallback>();
            var channel = GetCallbackChannel(param);

            var actionName = GetActionName(param);

            Dictionary<Guid, object> channelDic;

            if (channel != null)
            {
                logger.Debug("取得客户端Channel");
                lock (_callbackChannel)
                {
                    logger.Debug("通道缓存加锁完毕");
                    if (!_callbackChannel.ContainsKey(actionName))
                    {
                        logger.Debug(string.Format("注册通道类型:{0}", actionName));
                        channelDic = new Dictionary<Guid, object>();
                        _callbackChannel.Add(actionName, channelDic);
                    }
                    else
                    {
                        logger.Debug(string.Format("通道类型:{0}已存在", actionName));
                        channelDic = _callbackChannel[actionName];
                    }

                    if (!channelDic.ContainsKey(param.Token))
                    {
                        logger.Debug(string.Format("注册通道"));
                        channelDic.Add(param.Token, channel);
                    }
                }
            }

        }

        private static string GetActionName(CallBackParamDTO param)
        {
            string actionName =
                string.IsNullOrEmpty(param.ClientCallBack)
                    ? string.Empty
                    : param.ClientCallBack;
            return actionName;
        }

        /// <summary>
        /// 从服务上下文中取得回调通道信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static object GetCallbackChannel(CallBackParamDTO param)
        {
            OperationContext oc = OperationContext.Current;
            var fun = oc.GetType().GetMethod("GetCallbackChannel");
            //OperationContext.Current.GetCallbackChannel<IDualSVCallback>()
            var getCallbackChannel = fun.MakeGenericMethod(Type.GetType(param.ClientCallBack));
            var channel = getCallbackChannel.Invoke(oc, null);
            return channel;
        }

        /// <summary>
        /// 断开双向通讯连接
        /// </summary>
        /// <param name="param"></param>
        public void DisConnect(CallBackParamDTO param)
        {
            if (param == null)
            {
                logger.Error("参数为Null,直接返回,无法断开连接");
                return;
            }

            logger.Debug(string.Format("双向服务断开连接操作参数信息:{0}", param.ToString()));

            //var callBack =
            //  OperationContext.Current.GetCallbackChannel<IDualSVCallback>();
            //var channel = GetCallbackChannel(param);

            string actionName =
                   string.IsNullOrEmpty(param.ClientCallBack)
                   ? string.Empty
                   : param.ClientCallBack;

            Dictionary<Guid, object> channelDic;

            if (!string.IsNullOrEmpty(param.ClientCallBack))
            {
                logger.Debug("取得客户端Channel");
                lock (_callbackChannel)
                {
                    logger.Debug("通道缓存加锁完毕");
                    if (!_callbackChannel.ContainsKey(actionName))
                    {
                        logger.Debug(string.Format("通道类型:{0}尚未注册,服务结束", actionName));
                        return;
                    }
                    else
                    {
                        logger.Debug(string.Format("通道类型:{0}已存在", actionName));
                        channelDic = _callbackChannel[actionName];
                    }

                    if (channelDic.ContainsKey(param.Token))
                    {
                        logger.Debug(string.Format("释放通道"));
                        var channel = channelDic[param.Token];

                        channelDic.Remove(param.Token);
                    }
                }
            }
        }

        /// <summary>
        /// 执行服务实现
        /// </summary>
        /// <param name="param"></param>
        public void Excute(CallBackParamDTO param)
        {
            if (param == null)
            {
                logger.Error("参数为Null,直接返回,无法断开连接");
                return;
            }

            logger.Debug(string.Format("双向服务断开连接操作参数信息:{0}", param.ToString()));

            //var callBack =
            //  OperationContext.Current.GetCallbackChannel<IDualSVCallback>();
            var channelDic = string.IsNullOrEmpty(param.ClientCallBack)
                              ? null
                              : _callbackChannel[param.ClientCallBack];

            if (channelDic != null)
            {
                logger.Debug("Find Callback instance,Do Callback now!");
                if (param.Token != Guid.Empty)
                {
                    var channel = channelDic[param.Token];
                    var implementType = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                         where t.GetInterface(param.ClientCallBack) != null
                                         select t).FirstOrDefault();
                    if (implementType != null)
                    {
                        var action = implementType.GetMethod(param.Action);
                        if (action != null)
                        {
                            var imp = Assembly.GetExecutingAssembly().CreateInstance(implementType.FullName);
                            action.Invoke(imp, new object[] { param.Message });
                        }

                    }
                    //channel.GetType()
                }
                //channelDic.OnCallBackOneWay(new CallBackParamDTO());
                logger.Debug("Callback has done!");


            }
        }

        public static IList<object> GetClientChannel(CallBackParamDTO param)
        {

            if (param == null)
            {
                logger.Error("参数为Null,直接返回,无法查询通道列表");
                return null;
            }
            logger.Debug(string.Format("双向服务获取连接信息参数:{0}", param.ToString()));
            var actionName = GetActionName(param);
            if (_callbackChannel.ContainsKey(actionName))
            {
                logger.Debug(string.Format("双向服务获取连接信息,查询出已经注册的通道个数:{0}"
                    , _callbackChannel[actionName].Count));
                return _callbackChannel[actionName].Values.ToList();
            }
            return null;

        }
    }
}
