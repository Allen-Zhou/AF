using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.Service;
using System.Web.Caching;
using System.Web;
using TelChina.AF.Sys.DTO;

namespace TelChina.AF.Resource
{    
    public partial class ResourceService :ServiceBase,IResourceService
    {
        public Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 得到资源
        /// </summary>
        /// <param name="resourceType">类名称</param>
        /// <param name="resourceCode">属性名称</param>
        /// <returns></returns>
        public virtual string GetResource(string resourceType, string resourceCode)
        {
            Dictionary<string, string> list = list = cache.Get(resourceType) as Dictionary<string, string>;
            if (list == null)
            {
                list = new Dictionary<string, string>();
            }

            if (list.Count > 0)
            {
                if (list.ContainsKey(resourceCode))
                {
                    return list[resourceCode];
                }
                else
                {
                    string columnText = UserResource.ReturnResource(resourceType, resourceCode);
                    list.Add(resourceCode, columnText);
                    cache.Remove(resourceType);
                    cache.Insert(resourceType, list);
                    return columnText;
                }
            }
            else
            {
                string columnText = UserResource.ReturnResource(resourceType, resourceCode);
                list.Add(resourceCode, columnText);
                cache.Insert(resourceType, list);
                return columnText;
            }
        }

        /// <summary>
        /// 得到类下所有资源
        /// </summary>
        /// <param name="resourceType">类名称</param>
        public virtual Dictionary<string, string> GetResourcesByResourceType(string resourceType)
        {
            Dictionary<string, string> list = list = cache.Get(resourceType) as Dictionary<string, string>;
            if (list == null)
            {
                list = new Dictionary<string, string>();
            }

            if (list.Count == 0)
            {
                list = UserResource.ReturnResourcesByClassName(resourceType);
                cache.Insert(resourceType, list);
            }
            return list;
        }

        /// <summary>
        /// 得到类下所有资源
        /// </summary>
        /// <param name="resourceType">类名称</param>
        public virtual string GetTypeResource(string resourceType)
        {
            return UserResource.ReturnClassResource(resourceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public virtual string GetResource<TEntity>(System.Linq.Expressions.Expression<Func<TEntity, object>> whereExpression) where TEntity : DTOBase
        {
            string className = string.Empty;

            string columnName = string.Empty;

            var bodyExpr = whereExpression.Body as System.Linq.Expressions.MemberExpression;

            if (bodyExpr == null)
            {
                className = whereExpression.GetType().GetGenericArguments()[0].Name;

                return null;
            }
            if (bodyExpr.Member.DeclaringType.BaseType.Name == "DTOBase")
            {
                className = bodyExpr.Member.DeclaringType.FullName;
            }
            columnName = bodyExpr.Member.Name;

            return GetResource(className, columnName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enityBase"></param>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetResourcesByResourceType(DTOBase dtoBase)
        {
            string className = dtoBase.GetType().FullName;

            return GetResourcesByResourceType(className);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enityBase">实体</param>
        /// <returns></returns>
        public virtual string GetTypeResource(DTOBase dtoBase)
        {
            string className = dtoBase.GetType().FullName;

            return GetTypeResource(className);
        }
    }
}
