using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.DTO;
using System.Runtime.Serialization;

namespace TelChina.AF.Resource
{
    [DataContract]
    [Serializable]
    public partial class SysTemResourceDTO : DTOBase
    {
        /// <summary>
        /// 类名称
        /// </summary>
        [DataMember]
        public virtual string ResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 列名称
        /// </summary>
        [DataMember]
        public virtual string ResourceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 列描述
        /// </summary>
        [DataMember]
        public virtual string ResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        [DataMember]
        public virtual bool IsVisible
        {
            get;
            set;
        }

        /// <summary>
        /// 排序号
        /// </summary>
        [DataMember]
        public virtual string OrderNo
        {
            get;
            set;
        }
    }
}
