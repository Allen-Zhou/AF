using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using TelChina.AF.Service.AOP.AOPAttribute;
using System.Transactions;

namespace TelChina.AF.Service.AOP.CallHandler
{
    [ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class TransactionCallHandler : ICallHandler
    {
        private TransactionHandlerAttribute transAttrib;
        public TransactionCallHandler(TransactionHandlerAttribute transAttrib)
        {
            this.transAttrib = transAttrib;
        }
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            TransactionScope trans;
            switch (transAttrib.Option)
            {
                case TransactinOptionEnum.Required:
                    trans = new TransactionScope(TransactionScopeOption.Required);
                    break;
                case TransactinOptionEnum.RequiresNew:
                    trans = new TransactionScope(TransactionScopeOption.RequiresNew);
                    break;
                case TransactinOptionEnum.NotSuppoted:
                    trans = new TransactionScope(TransactionScopeOption.Suppress);
                    break;
                default:
                    {
                        trans = null;
                        break;
                    }
            }
            var result = getNext()(input, getNext);
            if (trans != null)
            {
                trans.Complete();
            }
            return result;
            //throw new NotImplementedException();
        }

        public int Order
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
