using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Linq.Expressions;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public class Resource
    {
        public Cache cache = HttpRuntime.Cache;
        
        public virtual string ReturnResource(string className, string columnName)
        {
            ReturnResource<User>(T => T.Address);

            Dictionary<string, string> list = cache.Get(className) as Dictionary<string, string>;
            if (list == null)
            {
                list = new Dictionary<string, string>();
            } 
            if (list.Count > 0)
            {
                if (list.ContainsKey(columnName))
                {
                    return list[columnName];
                }
                else
                {
                    string columnText = Useresource.ReturnResource(className, columnName);
                    list.Add(columnName, columnText);
                    cache.Remove(className);
                    cache.Insert(className, list);
                    return columnText;
                }
            }
            else
            {
                string columnText = Useresource.ReturnResource(className, columnName);
                list.Add(columnName, columnText);
                cache.Insert(className, list);
                return columnText;
            }
        }
        
        public virtual Dictionary<string, string> ReturnResourcesByClassName(string className)
        {

            Dictionary<string, string> list = cache.Get(className) as Dictionary<string, string>;
            if (list == null)
            {
                list = new Dictionary<string, string>();
            } 

            if (list.Count == 0)
            {
                list = Useresource.ReturnResourcesByClassName(className);
                cache.Insert(className, list);
            }
            return list;
        }

        public virtual string ReturnClassResource(string className)
        {
           
            return Useresource.ReturnClassResource(className);

           
        }

        public virtual string ReturnResource<TEntity>(Expression<Func<TEntity, object>> whereExpression) where TEntity : EntityBase
        {
            
            //if (lambdaExpressionType.GetGenericTypeDefinition() != typeof(Expression<>)) return null;

            //var funcType = lambdaExpressionType.GetGenericArguments()[0];
            //if (funcType.GetGenericTypeDefinition() != typeof(Func<,>)) return null;

            //var funcTypeArgs = funcType.GetGenericArguments();
            //if (funcTypeArgs[1] != typeof(bool)) return null;
            //return funcTypeArgs[0];

            return null;
        }
    }
}
