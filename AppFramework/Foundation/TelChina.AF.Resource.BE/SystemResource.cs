using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Resource
{
    public partial class SystemResource :EntityBase
    { 
        /// <summary>
        /// auto-generated:设置本实体上的字段默认值,应用开发扩展点
        /// </summary>
        protected override void SetDefaultValue()
        {

        }
        /// <summary>
        /// auto-generated:执行字段合法性检查,应用开发扩展点
        /// </summary>
        protected override void OnValidate()
        {

        }
        /// <summary>
        /// auto-generated:新增保存的前置条件
        /// </summary>
        protected override void OnInserting()
        {

        }
        /// <summary>
        /// auto-generated:新增保存的后置条件
        /// </summary>
        protected override void OnInserted()
        {

        }
        /// <summary>
        /// auto-generated:修改保存的前置条件
        /// </summary>
        protected override void OnUpdating()
        {

        }
        /// <summary>
        /// auto-generated:修改保存的后置条件
        /// </summary>
        protected override void OnUpdated()
        {

        }
        /// <summary>
        /// auto-generated:删除的前置条件
        /// </summary>
        protected override void OnDeleting()
        {

        }
        /// <summary>
        /// auto-generated:删除的后置条件
        /// </summary>
        protected override void OnDeleted()
        {

        }

        /// <summary>
        /// 得到资源
        /// </summary>
        /// <param name="className">类名称</param>
        /// <param name="columnName">属性名称</param>
        public static string ReturnResource(string className, string columnName)
        {
            string returnvalue = string.Empty;
            using (var repo = RepositoryContext.GetRepository())
            {
                var sysList = repo.GetAll<SystemResource>(T => T.ResourceType == className && T.ResourceCode == columnName, U => U.OrderNo, true);
                if (sysList.Count > 0)
                {
                    var sysResource = sysList[0];
                    returnvalue = sysResource.ResourceName;
                }
                return returnvalue;
            }
        }

        /// <summary>
        /// 得到类下所有资源
        /// </summary>
        /// <param name="className">类名称</param>
        public static IList<SystemResource> ReturnResourcesByClassName(string className)
        {
            using (var repo = RepositoryContext.GetRepository())
            {

                var sysList = repo.GetAll<SystemResource>(T => T.ResourceType == className && (T.ResourceCode != className),U=>U.OrderNo, true);

                return sysList;
            }
        }
        /// <summary>
        /// 得到类下所有资源
        /// </summary>
        /// <param name="className">类名称</param>
        public static string ReturnClassResource(string className)
        {
            string returnvalue = string.Empty;
            using (var repo = RepositoryContext.GetRepository())
            {

                var sysList = repo.GetAll<SystemResource>(T => T.ResourceType == className && (T.ResourceCode == className), null, true);
                if (sysList.Count > 0)
                {
                    var sysResource = sysList[0];
                    returnvalue = sysResource.ResourceName;
                }
                return returnvalue;
            }
        }

        public virtual IList<SystemResource> GetTypeResource()
        {
            IList<SystemResource> dtoList = new List<SystemResource>();
            using (var repo = RepositoryContext.GetRepository())
            {
                dtoList = repo.GetAll<SystemResource>(T => T.ResourceCode == T.ResourceType, U => U.OrderNo, true);
            }
            return dtoList;
        }

        public virtual IList<SystemResource> GetResourceByType(string resourceType)
        {
            IList<SystemResource> dtoList = new List<SystemResource>();
            using (var repo = RepositoryContext.GetRepository())
            {
                dtoList = repo.GetAll<SystemResource>(T => T.ResourceType == resourceType, null, true);
            }
            return dtoList;
        }
    }
}
