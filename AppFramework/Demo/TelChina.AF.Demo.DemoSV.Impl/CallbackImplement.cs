using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Util.Logging;
using System.Threading;

namespace TelChina.AF.Demo.DemoSV
{
    public class CallbackImplement : IDualSVCallback
    {
        private ILogger logger = LogManager.GetLogger("DualSVCallback");

       

        public void OnCallBack()
        {
            Thread.Sleep(10000);
        }

        public void OnCallBackOneWay(CallBackParamDTO param)
        {
            logger.Debug("On doing callback... ");
        }
    }
}
