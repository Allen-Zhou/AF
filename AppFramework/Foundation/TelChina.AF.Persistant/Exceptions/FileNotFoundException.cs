using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Persistant
{
    [Serializable]
    public class FileNotFoundException : UnhandledException
    {
        public FileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FileNotFoundException()
        {
        }

        public FileNotFoundException(string message)
            : base(message)
        {

        }
    }
}
