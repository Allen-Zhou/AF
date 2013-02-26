using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.AF.Persistant
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyMetaDataAttribute: Attribute
    {
        private bool allowNull = true;
        /// <summary>
        /// 是否非空
        /// </summary>
        public virtual bool AllowNull 
        {
            get
            {
                return allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public virtual int Length
        {
            get;
            set;
        }
    }
}
