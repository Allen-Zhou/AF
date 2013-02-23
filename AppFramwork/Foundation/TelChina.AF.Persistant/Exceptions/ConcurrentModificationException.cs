using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Persistant.Exceptions
{
    [Serializable]
    public class ConcurrentModificationException : BusinessException
    {
        public ConcurrentModificationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ConcurrentModificationException() { }
        public ConcurrentModificationException(string content)
        {
            BuildMessage(content);
        }

        public ConcurrentModificationException(string content, Exception ex)
        {
            BuildMessage(content);
            this.InnerException = ex;
        }

        private void BuildMessage(string content)
        {
            this.Content = content;
            this.OrderedProperties = new List<string>(1) { "Content" };
            this.MessageFormat = "并发修改异常,异常信息{0}";
        }

        /// <summary>
        ///异常详细信息
        /// </summary>
        public string Content
        {
            get
            {
                return (string)this.Data["Content"];
            }
            set
            {
                this.Data["Content"] = value;
            }
        }


    }
    
}
