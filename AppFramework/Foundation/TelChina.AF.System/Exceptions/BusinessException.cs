using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace TelChina.AF.Sys.Exceptions
{
    [Serializable]
    //[KnownType(typeof(BPExecption))]
    [KnownType(typeof(UnhandledException))]
    [KnownType(typeof(Exception))]
    [KnownType(typeof(Hashtable))]
    public class BusinessException : ExceptionBase
    {
         #region Constructor

        public BusinessException()
        {
        }
        public BusinessException(string message)
            : base(message)
        {
        }
        public BusinessException(Exception ex):base(ex)
        {
        }

        public BusinessException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public BusinessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    #endregion
    }
}
