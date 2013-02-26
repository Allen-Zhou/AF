using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.AF.Resource
{
    public enum ResourceState
    {
        /// <summary>
        /// 实体从数据库取出来的初始状态
        /// </summary>
        Unchanged,

        /// <summary>
        /// 新增的实体状态
        /// </summary>
        Inserting,

        /// <summary>
        /// 新增的实体状态
        /// </summary>
        Inserted,
        /// <summary>
        /// 待更新的实体状态
        /// </summary>
        Updating
    }
}
