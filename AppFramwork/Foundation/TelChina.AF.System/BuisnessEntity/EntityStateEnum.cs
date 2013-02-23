
namespace TelChina.AF.Persistant
{
    /// <summary>
    /// 实体状态,用于实体状态追踪
    /// </summary>
    public enum EntityStateEnum
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
        /// 待更新的实体状态
        /// </summary>
        Updating,

        /// <summary>
        /// 待删除的实体状态
        /// </summary>
        Deleting,

        /// <summary>
        /// 实体删除后状态
        /// </summary>
        Deleted
        
       /* /// <summary>
        /// 脱离仓储管理的实体状态
        /// </summary>
        Detached*/
    }
}
