using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Linq.Expressions;

namespace TelChina.AF.Demo.DemoSV
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    class MySubscriptionService : SubscriptionManager<IMyEvents>,
                                  IMySubscriptionService
    {
        
    }
}
