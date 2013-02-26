using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using TelChina.AF.Util.Logging;
using System.ServiceModel.Channels;
using TelChina.AF.Sys.Context;

namespace TelChina.AF.Sys.Service
{
    /// <summary>
    /// 服务上下文的管理器
    /// </summary>
    [DataContract]
    public class ServiceContextManager//< ServiceContext > where  ServiceContext  : class
    {
        private static ILogger _logger = LogManager.GetLogger("ServiceContextManager");

        /// <summary>
        /// 需要进行消息传递的上下文实例
        /// </summary>
        [DataMember]
        public readonly ServiceContext Value;
        #region  需要唯一确定一种类型,不一定非要用这种方式
        /// <summary>
        /// 用于Message契约传输的类型名称
        /// </summary>
        internal static string TypeName
        {
            get { return typeof(ServiceContext).Name; }
        }
        /// <summary>
        /// 用于Message契约传输的类型命名空间
        /// </summary>
        internal static string TypeNamespace
        {
            get { return typeof(ServiceContext).Namespace; }
        }

        //public static string SESSIONCOTEXTKEY
        //{
        //    get { return "AF.Service.ServiceSessionKey" + typeof(ServiceContext).FullName; }
        //}

        #endregion
        static ServiceContextManager()
        {
            //检查是否可被序列化
            //var  ServiceContext  = typeof( ServiceContext );
            //Debug.Assert(IsDataContract( ServiceContext ) ||  ServiceContext .IsSerializable);
        }

        /// <summary>
        /// 是否可以被序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsDataContract(Type type)
        {
            return type.GetCustomAttributes(typeof(DataContractAttribute), false).Count() > 0;
        }
        public ServiceContextManager(ServiceContext value)
        {
            this.Value = value;
        }
        public ServiceContextManager()
        {
            //var context = CallContext.GetData(SESSIONCOTEXTKEY) as  ServiceContext ;
            var context = ContextSession.Current;
            this.Value = context ?? default(ServiceContext);
        }

        public static ServiceContextManager Current
        {
            get
            {
                if (OperationContext.Current == null
                    || OperationContext.Current.IncomingMessageHeaders == null
                    || OperationContext.Current.IncomingMessageHeaders.FindHeader(TypeName, TypeNamespace) < 0
                    )
                {
                    return null;
                }
                ServiceContextManager result = null;
                try
                {
                    result = OperationContext.Current.IncomingMessageHeaders.
                      GetHeader<ServiceContextManager>(TypeName, TypeNamespace);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    //ToDo 这里需要记录日志,没有上下文信息需要给一个默认的
                    //throw;
                }
                return result;
            }
            set
            {
                Debug.Assert(OperationContext.Current != null);
                if (Current != null)
                {
                    throw new InvalidOperationException(string.Format("上下文已经存在:{0}", Current));
                }
                var header = new MessageHeader<ServiceContextManager>(value);
                OperationContext.Current.OutgoingMessageHeaders.
                    Add(header.GetUntypedHeader(TypeName, TypeNamespace));
            }
        }


        public override string ToString()
        {
            if (this.Value == null)
                return string.Format("Context类型:{0},{1}; 内容为空!",
                    TypeNamespace, TypeName);
            else
                return string.Format("Context类型:{0},{1}; 内容:{2}",
                    TypeNamespace, TypeName, Value.ToString());
        }

    }


}
