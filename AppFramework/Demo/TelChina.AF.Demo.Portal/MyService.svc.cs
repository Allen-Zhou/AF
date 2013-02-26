using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TelChina.AF.Sys.Service;

namespace TelChina.AF.Demo.Portal
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MyService" in code, svc and config file together.
    public class MyService : ServiceBase, IMyService
    {
        public void DoWork()
        {
            base.ServiceInvoke(() =>
            {
                Logger.Debug("执行服务");
                return 0;
            }
                                         );
        }
    }
}
