using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TelChina.AF.Persistant
{
    /// <summary>
    /// 实体Key,用于通用CRUD按照ID查询
    /// </summary>
    [DataContract]
    [Serializable]
    public class EntityKey
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        [DataMember]
        public string EntityType { get; set; }

        /// <summary>
        /// 当前实体key 是否合法
        /// </summary>
        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(this.EntityType) || this.ID == Guid.Empty; }
        }
    }
}
