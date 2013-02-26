using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TelChina.AF.Util.Logging;

namespace TelChina.AF.Demo.DemoSV
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DirectService" in both code and config file together.
    public class DirectService : IDirectService
    {
        private ILogger logger = LogManager.GetLogger("Service");
        public void DoWork()
        {
            logger.Debug("已经进入服务实现,正在执行服务...");
        }
    }
}
