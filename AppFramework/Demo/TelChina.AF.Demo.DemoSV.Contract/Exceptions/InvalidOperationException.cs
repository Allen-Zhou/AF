using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.Exceptions;
using System.Runtime.Serialization;

namespace TelChina.AF.Demo.DemoSV.Exceptions
{
    [Serializable]
    public class InvalidOperationException : ExceptionBase
    {
        public InvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public InvalidOperationException() { }
        public InvalidOperationException(string message)
            : base(message)
        {

        }

    }
}
