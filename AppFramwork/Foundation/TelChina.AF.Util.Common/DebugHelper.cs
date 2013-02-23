using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.AF.Util.Common
{
    public class DebugHelper
    {
        /// <summary>
        /// 是否将异常详细信息传播到客户端,用于服务的Behavior标签;
        /// 标签内容需要是编译期确定的常量值,故需要使用条件编译方式,不能取配置
        /// 在调试状况下应为True,便于跟踪异常;在生产环境中,应为False
        /// </summary>
        public const bool IncludeExceptionDetailInFaults=
       
         
#if DEBUG
                 true;
#else
                 false;
#endif
           
            //get
            //{
            //    var Config = ConfigHelper.GetConfigValue("Debug");
            //    if (!string.IsNullOrEmpty(Config) && Config.Contains("True"))
            //        return true;
            //    else
            //        return false;
            //}
       
    }
}
