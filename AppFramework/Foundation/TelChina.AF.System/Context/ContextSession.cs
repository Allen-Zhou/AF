using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.Service;
using System.Runtime.Remoting.Messaging;

namespace TelChina.AF.Sys.Context
{
    /// <summary>
    /// ServiceContext 缓存,在IIS进程中与应用服务器中不同,需要使用不同的实现方式
    /// </summary>
    public class ContextSession
    {
        static ContextSession()
        {

        }

        private static ISessionProvider _provider = new AppContextSessionProvider();
        /// <summary>
        /// 获取当前会话的用户登录的信息
        /// </summary>
        public static ServiceContext Current
        {
            get
            {
                //如果还没有就给出默认值
                return Provider.GetData();
            }

            set
            {
                Provider.SetData(value);
            }
        }

        public static ISessionProvider Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }
    }
}
