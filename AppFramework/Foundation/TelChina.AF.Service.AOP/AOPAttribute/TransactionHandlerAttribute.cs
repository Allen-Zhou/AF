using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using TelChina.AF.Service.AOP.CallHandler;

namespace TelChina.AF.Service.AOP.AOPAttribute
{
  

    public class TransactionHandlerAttribute : HandlerAttribute
    {
        public TransactinOptionEnum Option { get; set; }

        public override ICallHandler CreateHandler(Microsoft.Practices.Unity.IUnityContainer container)
        {
            return new TransactionCallHandler(this);
        }
    }
}
