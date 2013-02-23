
namespace TelChina.AF.Persistant
{
    public interface IPersistableObject
    {
        /// <summary>
        /// 设置本实体上的字段默认值
        /// </summary>
        void SetDefaultValue();
        /// <summary>
        /// 执行字段合法性检查
        /// </summary>
        void OnValidate();

        /// <summary>
        /// 实体状态
        /// </summary>
        EntityStateEnum SysState
        {
            get;
            set;
        }
        /// <summary>
        /// 新增保存的前置条件
        /// </summary>
        void OnInserting();
        /// <summary>
        /// 新增保存的后置条件
        /// </summary>
        void OnInserted();
        /// <summary>
        /// 修改保存的前置条件
        /// </summary>
        void OnUpdating();
        /// <summary>
        /// 修改保存的后置条件
        /// </summary>
        void OnUpdated();
        /// <summary>
        /// 删除的前置条件
        /// </summary>
        void OnDeleting();
        /// <summary>
        /// 删除的后置条件
        /// </summary>
        void OnDeleted();
    }
}
