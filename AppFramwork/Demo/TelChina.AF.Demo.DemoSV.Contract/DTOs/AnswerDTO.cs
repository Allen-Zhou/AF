using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TelChina.AF.Sys.Serialization;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    [DataContract(Namespace = @"http://schemas.telchina.org/2013/2/Entity", Name = "AnswerDTO")]
    [DataContractResolverAttribute(TypeAssembly = "TelChina.AF.Demo",
        TypeFullName = "TelChina.AF.Demo.Answer")]
    [Serializable]
    public partial class AnswerDTO : DTOBase
    {
        private String _name;

        private Guid? _question_ID;

        [DataMember(Order = 0)]
        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {

                    _name = value;

                }
            }
        }
        //[DataMember(Order = 1, Name = "Question")]
        public Guid? Question_ID
        {
            get
            {
                return this._question_ID;
            }
            set
            {
                if (this._question_ID != value)
                {

                    _question_ID = value;

                }
            }
        }


        public override string ToString()
        {
            var result = new StringBuilder(128);
            result.Append(string.Format("EnitityType:{0},Propertys:[{1}:{2},{3}:{4}]", "Answer", "Name", this.Name,
                                        "Question", "Null"));
            result.Append(base.ToString());

            return result.ToString();
        }
    }
}




