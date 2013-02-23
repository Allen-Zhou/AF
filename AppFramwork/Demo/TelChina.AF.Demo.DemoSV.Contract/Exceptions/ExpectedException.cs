using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.Exceptions;
using System.Runtime.Serialization;

namespace TelChina.AF.Demo.DemoSV.Exceptions
{
    [Serializable]
    public class ExpectedException : ExceptionBase
    {
        protected ExpectedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ExpectedException()
            : base("这是测试用例期待的异常")
        {
        }
    }
}
