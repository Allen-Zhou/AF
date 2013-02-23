
namespace TelChina.AF.Sys.Service
{
    /// <summary>
    /// 服务实现策略的基类,定义了服务实现的入口
    /// </summary>
    public abstract class StrategyBase
    {
        /// <summary>
        /// 业务逻辑实现
        /// </summary>
        /// <returns>应根据模型中定义返回相应的值,
        /// 如果返回值定义为void,应在实现中返回null,
        /// 框架会根据模型自动转换到适当的类型
        /// </returns>
        public abstract object Do();
    }
}
