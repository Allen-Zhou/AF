using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Demo.DemoSV.Interfaces;
using System.ServiceModel;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;

namespace TelChina.AF.Demo.DemoSV
{

    public class DualSVBase<T> where T : class
    {
        //static void PublishPersistent(string methodName, object[] args)
        //{
        //    T[] subscribers = SubscriptionManager<T>.GetPersistentList(methodName);
        //    Publish(subscribers, true, methodName, args);
        //}

        //protected static void FireEvent(params object[] args)
        //{
        //    string action = OperationContext.Current.IncomingMessageHeaders.Action;
        //    string[] slashes = action.Split('/');
        //    string methodName = slashes[slashes.Length - 1];
        //    PublishPersistent(methodName, args);
        //}

        protected static void FireEvent(string action, params object[] parameters)
        {
            if (string.IsNullOrEmpty(action))
            {
                return;
            }

            var param = new CallBackParamDTO()
                            {
                                ClientCallBack = string.Format("{0},{1}",
                                    typeof(T).FullName,
                                    typeof(T).Assembly.FullName),
                                Action = action,
                                Message = parameters[0].ToString(),
                                Token = Guid.Empty
                            };
            var channels = DualSV.GetClientChannel(param);
            if (channels != null)
            {
                foreach (var item in channels)
                {
                    var channel = item as IDualSVCallback;
                    if (channel != null)
                    {
                        channel.OnCallBackOneWay(param);
                    }
                }
            }
        }

        static void Publish(T[] subscribers, bool closeSubscribers, string methodName,
                                                                      object[] args)
        {
            WaitCallback fire = (subscriber) =>
            {
                Invoke(subscriber as T, methodName, args);
                if (closeSubscribers)
                {
                    using (subscriber as IDisposable)
                    { }
                }
            };
            Action<T> queueUp = (subscriber) => ThreadPool.QueueUserWorkItem(fire, subscriber);
            Array.ForEach(subscribers, queueUp);
        }
        static void Invoke(T subscriber, string methodName, object[] args)
        {
            Debug.Assert(subscriber != null);
            Type type = typeof(T);
            MethodInfo methodInfo = type.GetMethod(methodName);
            try
            {
                methodInfo.Invoke(subscriber, args);
            }
            catch
            { }
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BizDualSVImpl : DualSVBase<IBizDualContract>, IBizDualContract
    {
        /// <summary>
        /// 触发回调
        /// </summary>
        /// <param name="str"></param>
        public void ServiceInvoke(string str)
        {
            DoCallBack(str);
        }

        public void DoCallBack(string str)
        {
            FireEvent("DoCallBack", str);
        }
    }
}
