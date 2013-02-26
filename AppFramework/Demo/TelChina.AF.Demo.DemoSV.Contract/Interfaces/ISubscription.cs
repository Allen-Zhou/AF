using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace TelChina.AF.Demo.DemoSV
{
    [ServiceContract]
    public interface ISubscriptionService
    {
        [OperationContract]
        void Subscribe(string eventOperation);
        [OperationContract]
        void Unsubscribe(string eventOperation);
    }

    //CallBack EventContract
    public interface IMyEvents
    {
        [OperationContract(IsOneWay = true)]
        void OnEvent1();
        [OperationContract(IsOneWay = true)]
        void OnEvent2(int number);
        [OperationContract(IsOneWay = true)]
        void OnEvent3(int number, string text);
    }

    //Service
    [ServiceContract(CallbackContract = typeof(IMyEvents))]
    public interface IMySubscriptionService : ISubscriptionService
    { }

    // Framwork
    public abstract class SubscriptionManager<T> where T : class
    {
        //More members

        static Dictionary<string, List<T>> m_TransientStore;
        static SubscriptionManager()
        {
            m_TransientStore = new Dictionary<string, List<T>>();
            string[] methods = GetOperations();
            Action<string> insert = (methodName) =>
                                    {
                                        m_TransientStore.Add(methodName, new List<T>());
                                    };
            Array.ForEach(methods, insert);
        }
        static string[] GetOperations()
        {
            MethodInfo[] methods = typeof(T).GetMethods(BindingFlags.Public |
                                                        BindingFlags.FlattenHierarchy |
                                                        BindingFlags.Instance);
            List<string> operations = new List<string>(methods.Length);
            Action<MethodInfo> add = (method) =>
            {
                Debug.Assert(!operations.Contains(method.Name));
                operations.Add(method.Name);
            };
            Array.ForEach(methods, add);
            return operations.ToArray();
        }
        static void AddTransient(T subscriber, string eventOperation)
        {
            lock (typeof(SubscriptionManager<T>))
            {
                List<T> list = m_TransientStore[eventOperation];
                if (list.Contains(subscriber))
                {
                    return;
                }
                list.Add(subscriber);
            }
        }
        static void RemoveTransient(T subscriber, string eventOperation)
        {
            lock (typeof(SubscriptionManager<T>))
            {
                List<T> list = m_TransientStore[eventOperation];
                list.Remove(subscriber);
            }
        }
        public void Subscribe(string eventOperation)
        {
            lock (typeof(SubscriptionManager<T>))
            {
                T subscriber = OperationContext.Current.GetCallbackChannel<T>();
                if (String.IsNullOrEmpty(eventOperation) == false)
                {
                    AddTransient(subscriber, eventOperation);
                }
                else
                {
                    string[] methods = GetOperations();
                    Action<string> addTransient = (methodName) =>
                    {
                        AddTransient(subscriber, methodName);
                    };
                    Array.ForEach(methods, addTransient);
                }
            }
        }
        public static T[] GetPersistentList(string methodName)
        {
            return m_TransientStore[methodName].ToArray();
        }
        public static T[] GetTransientList(string methodName)
        {
            return GetPersistentList(methodName);
        }
        public void Unsubscribe(string eventOperation)
        {
            lock (typeof(SubscriptionManager<T>))
            {
                T subscriber = OperationContext.Current.GetCallbackChannel<T>();
                if (String.IsNullOrEmpty(eventOperation) == false)
                {
                    RemoveTransient(subscriber, eventOperation);
                }
                else
                {
                    string[] methods = GetOperations();
                    Action<string> removeTransient = (methodName) =>
                    {
                        RemoveTransient(subscriber, methodName);
                    };
                    Array.ForEach(methods, removeTransient);
                }
            }
        }
    }


    public abstract class PublishService<T> where T : class
    {
        static void PublishPersistent(string methodName, object[] args)
        {
            T[] subscribers = SubscriptionManager<T>.GetPersistentList(methodName);
            Publish(subscribers, true, methodName, args);
        }
        static void PublishTransient(string methodName, object[] args)
        {
            T[] subscribers = SubscriptionManager<T>.GetTransientList(methodName);
            Publish(subscribers, false, methodName, args);
        }
        protected static void FireEvent(params object[] args)
        {
            string action = OperationContext.Current.IncomingMessageHeaders.Action;
            string[] slashes = action.Split('/');
            string methodName = slashes[slashes.Length - 1];
            FireEvent(methodName, args);
        }
        static void FireEvent(string methodName, object[] args)
        {
            PublishPersistent(methodName, args);
            PublishTransient(methodName, args);
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
    class MyPublishService : PublishService<IMyEvents>, IMyEvents
    {
        public void OnEvent1()
        {
            FireEvent();
        }
        public void OnEvent2(int number)
        {
            FireEvent(number);
        }
        public void OnEvent3(int number, string text)
        {
            FireEvent(number, text);
        }
    }
}
