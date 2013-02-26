﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Iesi.Collections.Generic;
using System;
using TelChina.AF.Persistant;
using System.Collections.Generic;

namespace TelChina.AF.Demo
{
    public partial class Useresource : EntityBase
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
                var list = repo.GetAll<Useresource>(T => T.ClassName == className && T.ColumnName == columnName, null, true);
                if (list.Count > 0)
                {
                    var userResource = list[0];
                    returnvalue = userResource.ColumnDescribe;
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
                var list = repo.GetAll<Useresource>(T => T.ClassName == className && (T.ColumnName != null || T.ColumnName != ""), null, true);
                if (list.Count > 0)
                {
                    foreach (var userResource in list)
                    {
                        if (!returnvalue.ContainsKey(userResource.ColumnName))
                        {
                            returnvalue.Add(userResource.ColumnName, userResource.ColumnDescribe);
                        }
                    }
                }
                
                var sysList = SystemResource.ReturnResourcesByClassName(className);

                foreach (var sysResource in sysList)
                {
                    if (!returnvalue.ContainsKey(sysResource.ColumnName))
                    {
                        returnvalue.Add(sysResource.ColumnName, sysResource.ColumnDescribe);
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
                var list = repo.GetAll<Useresource>(T => T.ClassName == className && (T.ColumnName == null || T.ColumnName == ""), null, true);
                if (list.Count > 0)
                {
                    var userResource = list[0];
                    returnvalue = userResource.ColumnDescribe;
                }
                else
                {
                    returnvalue = SystemResource.ReturnClassResource(className);
                }
                return returnvalue;
            }
        }
    }
}


