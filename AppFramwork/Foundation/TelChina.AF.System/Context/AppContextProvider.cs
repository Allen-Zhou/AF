using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.Service;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Runtime.Serialization;

namespace TelChina.AF.Sys.Context
{

    public interface ISessionProvider
    {
        void SetData(ServiceContext ctx);
        ServiceContext GetData();
    }

    [Serializable]
    public class AppContextSessionProvider : Dictionary<string, object>, ISessionProvider
    {
        /// <summary>
        /// 需要满足反序列化的要求
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public AppContextSessionProvider(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public AppContextSessionProvider()
            : base()
        {

        }
        private static AppContextSessionProvider Current
        {
            get
            {
                if (null != HttpContext.Current)
                {
                    if (null == HttpContext.Current.Session[ContextKey])
                    {
                        HttpContext.Current.Session[ContextKey] = new AppContextSessionProvider();
                    }

                    return HttpContext.Current.Session[ContextKey] as AppContextSessionProvider;
                }

                if (null == CallContext.GetData(ContextKey))
                {
                    CallContext.SetData(ContextKey, new AppContextSessionProvider());
                }
                return CallContext.GetData(ContextKey) as AppContextSessionProvider;
            }
        }
        public const string ContextKey = "TelChina.AF.Context.AppContextSessionProvider";
        public static string SESSIONKEY = "TelChina.AF.Context.AppContextSession";

        public void SetData(ServiceContext ctx)
        {
            Current[SESSIONKEY] = ctx;
        }

        public ServiceContext GetData()
        {
            //保证上下文缓存中一定有数据
            if (!Current.ContainsKey(SESSIONKEY))
            {
                Current[SESSIONKEY] = new ServiceContext();
            }
            var ctx = Current[SESSIONKEY] as ServiceContext;
            //var ctx = CallContext.GetData(SESSIONKEY) as ServiceContext;
            return ctx ?? new ServiceContext();
        }
    }
}
