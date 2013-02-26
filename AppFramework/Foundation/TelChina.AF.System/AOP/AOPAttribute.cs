using System;

namespace TelChina.AF.Sys.AOP
{
    /// <summary>
    /// AOP frame 基类,定义AOP扩展接口
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Method)]
    public abstract class AOPAttribute : Attribute
    {
        protected AOPAttribute()
        {

        }
        /// <summary>
        /// 在业务执行前需要处理的当前关注点的逻辑
        /// </summary>
        public abstract void BeforeInvoke(Service.ServiceBase svObj);
        /// <summary>
        /// 在业务执行后需要处理的当前关注点的逻辑
        /// </summary>
        public abstract void AfterInvoke(Service.ServiceBase svObj);

    }
}
