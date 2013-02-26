using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TelChina.AF.Persistant;
using TelChina.AF.Sys.DTO;

namespace TelChina.AF.Persistant
{
    [DataContract]
    [Serializable]
    public class BizDTO : DTOBase
    {

        #region 属性


        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Order = 0)]
        public virtual Guid ID
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember(Order = 1)]
        public virtual DateTime CreatedOn
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember(Order = 2)]
        public virtual string CreatedBy
        {
            get;
            set;
        }
        /// <summary>
        /// 最近一次更新时间
        /// </summary>
        [DataMember(Order = 3)]
        public virtual DateTime UpdatedOn
        {
            get;
            set;
        }
        /// <summary>
        /// 最近一次更新人
        /// </summary>
        [DataMember(Order = 4)]
        public virtual string UpdatedBy
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        [DataMember(Order = 5)]
        public virtual int SysVersion
        {
            get;
            set;
        }
        /// <summary>
        /// 实体状态 
        /// </summary>
        [DataMember(Order = 6)]
        public EntityStateEnum SysState
        {
            get;
            set;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(string.Format(",ID:{0},SysVersion:{1},SysState:{2}",
                this.ID, this.SysVersion, this.SysState));
            return result.ToString();
        }
        #endregion

        //protected DTOBase(SerializationInfo info, StreamingContext context)
        //{
        //}

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
