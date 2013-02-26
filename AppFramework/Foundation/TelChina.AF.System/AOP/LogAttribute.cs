using System.Text;
using TelChina.AF.Util.Logging;

namespace TelChina.AF.Sys.AOP
{
    /// <summary>
    /// 日志帧
    /// </summary>
    public class LogAttribute : AOPAttribute
    {
        ILogger logger = null;
        public LogAttribute()
            : base()
        {
            this.logger = LogManager.GetLogger("TelChina.AF.System.AOP.ServiceEngine");
        }

        public override void BeforeInvoke(Service.ServiceBase svObj)
        {
            StringBuilder result = new StringBuilder(1024);
            result.Append(string.Format("服务开始执行,服务类型:{0},参数信息:{1}", svObj.GetType().FullName, GetParamInfo(svObj)));
            logger.Debug(result.ToString());
        }

        private string GetParamInfo(Service.ServiceBase svObj)
        {
            return "暂不支持记录参数信息...";
        }

        public override void AfterInvoke(Service.ServiceBase svObj)
        {
            logger.Debug("服务执行完成记录日志");
        }
    }
}
