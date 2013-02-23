using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Collections;
using TelChina.AF.Sys.Exceptions;


namespace TelChina.AF.Demo.DemoSV
{

  
    [Serializable]
    public class DemoSVExecption : ExceptionBase
    {
        public DemoSVExecption(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public DemoSVExecption() { }
        public DemoSVExecption(string containt)
        {
            this.Containt = containt;
            this.OrderedProperties = new List<string>(1) { "Containt" };
            this.MessageFormat = "SV调用异常,异常信息{0}";
        }

        /// <summary>
        ///异常详细信息
        /// </summary>
        public string Containt
        {
            get
            {
                return (string)this.Data["Containt"];
            }
            set
            {
                this.Data["Containt"] = value;
            }
        }


    }
}
