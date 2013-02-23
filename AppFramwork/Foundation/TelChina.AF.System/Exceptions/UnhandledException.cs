using System;
using System.Runtime.Serialization;

namespace TelChina.AF.Sys.Exceptions
{
    [Serializable]
    public class UnhandledException : ExceptionBase
    {
        public UnhandledException() : base() { }
        public UnhandledException(Exception ex)
            : base(ex)
        {
            this.MessageFormat = "未知非系统异常";
        }
        public UnhandledException(SerializationInfo info, StreamingContext context) : base(info, context) { }


        public UnhandledException(string message)
            : base(message)
        {

        }
        public UnhandledException(string message, Exception ex)
            : base(message, ex)
        {
            this.MessageFormat = "未知非系统异常";
        }
    }
}
