using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Resource
{
    public partial class UserResource : EntityBase
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
                var list = repo.GetAll<UserResource>(T => T.ResourceType == className && T.ResourceCode == columnName,U=>U.OrderNo, true);
                if (list.Count > 0)
                {
                    var userResource = list[0];
                    returnvalue = userResource.ResourceName;
                }
                else
                {
                    returnvalue = SystemResource.ReturnResource(className, columnName);
                }
                return returnvalue;
            }
        }

        /// <summary>
        /// 得到类下所有资源
        /// </summary>
        /// <param name="className">类名称</param>
        public static Dictionary<string, string> ReturnResourcesByClassName(string className)
        {
            Dictionary<string, string> returnvalue = new Dictionary<string, string>(); ;
            using (var repo = RepositoryContext.GetRepository())
            {
                var list = repo.GetAll<UserResource>(T => T.ResourceType == className && (T.ResourceCode != className), null, true);
                if (list.Count > 0)
                {
                    foreach (var userResource in list)
                    {
                        if (!returnvalue.ContainsKey(userResource.ResourceCode))
                        {
                            returnvalue.Add(userResource.ResourceCode, userResource.ResourceName);
                        }
                    }
                }

                var sysList = SystemResource.ReturnResourcesByClassName(className);

                foreach (var sysResource in sysList)
                {
                    if (!returnvalue.ContainsKey(sysResource.ResourceCode))
                    {
                        returnvalue.Add(sysResource.ResourceCode, sysResource.ResourceName);
                    }
                }

            }
            return returnvalue;
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
                var list = repo.GetAll<UserResource>(T => T.ResourceType == className && (T.ResourceCode == className),U=>U.OrderNo, true);
                if (list.Count > 0)
                {
                    var userResource = list[0];
                    returnvalue = userResource.ResourceName;
                }
                else
                {
                    returnvalue = SystemResource.ReturnClassResource(className);
                }
                return returnvalue;
            }
        }

        public virtual IList<UserResource> GetTypeResource()
        {
            IList<UserResource> dtoList = new List<UserResource>();
            using (var repo = RepositoryContext.GetRepository())
            {
                dtoList = repo.GetAll<UserResource>(T => (T.ResourceCode == T.ResourceType), null, true);                
            }
            return dtoList;
        }

        public virtual IList<UserResource> GetResourceByType(string resourceType)
        {
            IList<UserResource> dtoList = new List<UserResource>();
            using (var repo = RepositoryContext.GetRepository())
            {
                dtoList = repo.GetAll<UserResource>(T => T.ResourceType == resourceType,U=>U.OrderNo, true);
            }
            
            return dtoList;
        }

        public virtual void UpdateResource(List<UserResource> userResources)
        {

        }
    }
}
