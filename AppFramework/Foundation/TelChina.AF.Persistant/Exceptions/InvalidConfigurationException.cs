using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Persistant
{
    [Serializable]
    public class InvalidConfigurationException : ExceptionBase
    {
        public InvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidConfigurationException()
        {
        }

        public InvalidConfigurationException(string message)
            : base(message)
        {

        }
    }
}
