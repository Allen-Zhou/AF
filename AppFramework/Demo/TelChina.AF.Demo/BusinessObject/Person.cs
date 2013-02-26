using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace TelChina.AF.Demo
{
    public partial class Person:EntityBase
    {        

        protected override void SetDefaultValue()
        {
            if (string.IsNullOrEmpty(this.Code))
            {
                this.Code = "Person-1111";
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                this.Name = "张三";
            }
        }

        protected override void OnValidate()
        {
            if (this.Code == null || this.Code.Trim().Length == 0)
            {
                throw new Exception("编码不能为空");
            }
            else if (this.Code.Trim().Length > 12)
            {
                throw new Exception("编码不能长于12位");
            }

            if (this.Name == null || this.Name.Trim().Length == 0)
            {
                throw new Exception("名称不能为空");
            }
            else if (this.Name.Trim().Length > 50)
            {
                throw new Exception("名称不能长于50位");
            }

            //if (this.Department == null)
            //{
            //    throw new Exception("AA02003");
            //}

            if (this.Code.Trim().Contains(";"))
            {
                //编码中不能出现半角分号
                throw new Exception("编码中不能出现半角分号");
            }
            if (this.Code.Trim().Contains("；"))
            {
                //编码中不能出现全角分号
                throw new Exception("编码中不能出现全角分号");
            }           
            //3. 调用Department.Finder.FindByID,
            //   确定所属部门存在，且为未被停用的末级部门.
            //Department Depar = Department.Finder.FindById(this.Department.ID);
            //if (Depar == null)
            //{
            //    throw new Exception("部门不存在，请重新输出");
            //}            
            //else if (!Depar.IsEndNode)
            //{
            //    throw new Exception("部门为非末级部门，不能使用");
            //}
            //if (Depar.Disabled)
            //{
            //    throw new Exception("部门已经停用部门，不能使用");
            //}
        }

        protected override void OnDeleted()
        {
            
        }

        protected override void OnDeleting()
        {
            if (this.Disabled)
            {
                throw new Exception("该人员被停用不能删除，请先启用  ");
            }
        }

        protected override void OnInserted()
        {
            //会死循环，不用此方法
            //LogClass logClass = new LogClass();
            //logClass.SaveLog("Inserting");
        }

        protected override void OnInserting()
        {
            if (this.Code=="Person-1171")
            {                
                throw new Exception("编码存在，请重新输入！");
            }
        }

        protected override void OnUpdated()
        {
            
        }

        protected override void OnUpdating()
        {
            if (this.Code == "Person-1171")
            {

                throw new Exception("编码存在，请重新输入！");
            }
        }

        /// <summary>
        /// 创建新的实体
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="disabled"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Person CreatePerson(string code, String name, bool disabled, Guid id)
        {
            var person = new Person();
            person.Birthday = DateTime.Now;
            person.Telphone = "15264183573";
            person.Address = "济南";
            person.PostCode = "250014";
            person.Gender = true;
            person.Disabled = disabled;
            person.NativePlace = "汉";
            if (id != Guid.Empty)
            {
                person.ID = id;
            }
            person.Code = code;
            person.Name = name;
            person.CreatedOn = DateTime.Now;
            person.CreatedBy = "DS";
            person.UpdatedOn = DateTime.Now;
            person.UpdatedBy = "DS";
            return person;
        }

        /// <summary>
        /// 返回SQLCommand
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static SqlCommand InsertBySQLCommand(Person person,string opertion)
        {
            SqlCommand sqlCommand = new SqlCommand();
            if (opertion == "Delete")
            {
                sqlCommand.CommandText = @"Delete from [Person] Where ID =@ID";
                sqlCommand.Parameters.Add(new SqlParameter("@ID", person.ID));
                return sqlCommand;
            }
            
            if (opertion == "Insert")
            {
                sqlCommand.CommandText = @"INSERT INTO [Person] (SysVersion, Code, Name, Telphone, Address, PostCode, 
            Gender, Disabled, NativePlace, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, ID) 
            VALUES (@SysVersion, @Code, @Name, @Telphone, @Address, @PostCode, @Gender, @Disabled, 
            @NativePlace, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy, @ID)";
            }
            if (opertion == "Update")
            {
                sqlCommand.CommandText = @"Update [Person] Set SysVersion =@SysVersion, Code=@Code, Name=@Name, Telphone=@Telphone, 
            Address=@Address, PostCode=@PostCode, Gender=@Gender, Disabled= @Disabled, NativePlace=@NativePlace,
            CreatedOn=@CreatedOn, CreatedBy=@CreatedBy, UpdatedOn=@UpdatedOn, UpdatedBy=@UpdatedBy Where ID=@ID";
            }
            sqlCommand.Parameters.Add(new SqlParameter("@SysVersion", person.SysVersion));
            sqlCommand.Parameters.Add(new SqlParameter("@Code", person.Code));
            sqlCommand.Parameters.Add(new SqlParameter("@Name", person.Name));
            sqlCommand.Parameters.Add(new SqlParameter("@Telphone", person.Telphone));
            sqlCommand.Parameters.Add(new SqlParameter("@Address", person.Address));
            sqlCommand.Parameters.Add(new SqlParameter("@PostCode", person.PostCode));
            sqlCommand.Parameters.Add(new SqlParameter("@Gender", person.Gender));
            sqlCommand.Parameters.Add(new SqlParameter("@Disabled", person.Disabled));
            sqlCommand.Parameters.Add(new SqlParameter("@NativePlace", person.NativePlace));
            sqlCommand.Parameters.Add(new SqlParameter("@CreatedOn", person.CreatedOn));
            sqlCommand.Parameters.Add(new SqlParameter("@CreatedBy", person.CreatedBy));
            sqlCommand.Parameters.Add(new SqlParameter("@UpdatedOn", person.UpdatedOn));
            sqlCommand.Parameters.Add(new SqlParameter("@UpdatedBy", person.UpdatedBy));
            sqlCommand.Parameters.Add(new SqlParameter("@ID", person.ID));
            return sqlCommand;

        }
        #region Finder类
        public class Finder
        {
            #region 公开方法
            /// <summary>
            /// 通过条件查询 返回IList
            /// </summary>
            /// <returns>IList</returns>
            public static IList<Person> FindList()
            {
                var repo = RepositoryContext.GetRepository();
                IList<Person> resultList;
                resultList = repo.GetAll<Person>();

                return resultList;
            }

            /// <summary>
            /// 通过条件查询 返回IList
            /// </summary>
            /// <param name="whereExpression">条件表达式</param>
            /// <param name="orderByExpression">排序字段表达式</param>
            /// <param name="ascending">升序排列</param>
            /// <returns>IList</returns>
            public static IList<Person> FindList(Expression<Func<Person, bool>> whereExpression,
                                           Expression<Func<Person, object>> orderByExpression, bool ascending)
            {
                var repo = RepositoryContext.GetRepository();
                IList<Person> resultList;
                resultList = repo.GetAll<Person>(whereExpression, orderByExpression, ascending);
                return resultList;
            }

            /// <summary>
            /// 通过条件查询 返回IList
            /// </summary>
            /// <param name="pageIndex">页码</param>
            /// <param name="pageCount">该页行数</param>
            /// <param name="whereExpression">条件表达式</param>
            /// <param name="orderByExpression">排序字段表达式</param>
            /// <param name="ascending">升序排列</param>
            /// <returns>IList</returns>
            public static IList<Person> FindList(int pageIndex, int pageCount, Expression<Func<Person, bool>> whereExpression,
                                              Expression<Func<Person, object>> orderByExpression, bool ascending)
            {
                var repo = RepositoryContext.GetRepository();
                IList<Person> resultList;
                resultList = repo.GetAll<Person>(whereExpression, orderByExpression, ascending,pageIndex, pageCount);
                return resultList;
            }
            /*


             */

            /// <summary>
            /// 通过条件查询 返回Department
            /// </summary>
            /// <param name="id">Guid</param>
            /// <returns>Department</returns>
            public static Person FindById(Guid id)
            {
                if (id == null)
                {
                    return null;
                }
                var repo = RepositoryContext.GetRepository();
                Person resultDto = repo.GetByID<Person>(id);

                return resultDto;
            }

            ///// <summary>
            ///// 通过条件查询 返回首张
            ///// </summary>
            ///// <param name="totalcount">所有记录数</param>
            ///// <param name="whereExpression">条件表达式</param>
            ///// <param name="orderByExpression">排序字段表达式</param>
            ///// <param name="ascending">升序排列</param>
            ///// <returns>DepartmentDTO</returns>
            //public static Person FindFirst(out int totalCount, Expression<Func<Person, bool>> whereExpression,
            //                                  Expression<Func<Person, object>> orderByExpression, bool ascending)
            //{
            //    var repo = RepositoryContext.GetRepository();
            //    var list = repo.GetAll(whereExpression, orderByExpression, ascending);
            //    totalCount = list.Count;
            //    return list.First();

            //}

            ///// <summary>
            ///// 通过条件查询 返回最后一张
            ///// </summary>
            ///// <param name="totalcount">所有记录数</param>
            ///// <param name="whereExpression">条件表达式</param>
            ///// <param name="orderByExpression">排序字段表达式</param>
            ///// <param name="ascending">升序排列</param>
            ///// <returns>Department</returns>
            //public static Person FindLast(out int totalCount, Expression<Func<Person, bool>> whereExpression,
            //                                  Expression<Func<Person, object>> orderByExpression, bool ascending)
            //{
            //    var repo = RepositoryContext.GetRepository();
            //    var list = repo.GetAll(whereExpression, orderByExpression, ascending);
            //    totalCount = list.Count;
            //    return list.Last();
            //}

            ///// <summary>
            ///// 通过条件查询 返回下一张
            ///// </summary>
            ///// <param name="totalcount">所有记录数</param>
            ///// <param name="dto">当前实体</param>
            ///// <param name="whereExpression">条件表达式</param>
            ///// <param name="orderByExpression">排序字段表达式</param>
            ///// <param name="ascending">升序排列</param>
            ///// <returns>DepartmentDTO</returns>
            //public static Person FindNext(out int totalCount, Department dto, Expression<Func<Person, bool>> whereExpression,
            //                                  Expression<Func<Person, object>> orderByExpression, bool ascending)
            //{

            //    var repo = RepositoryContext.GetRepository();
            //    var list = repo.GetAll(whereExpression, orderByExpression, ascending);
            //    totalCount = list.Count;
            //    int i = 0;
            //    foreach (Person person in list)
            //    {
            //        i = i + 1;
            //        if (person.ID == dto.ID)
            //        {
            //            break;
            //        }
            //    }
            //    if (i == totalCount)
            //        return null;
            //    return list[i];

            //}

            ///// <summary>
            ///// 通过条件查询 返回上一张
            ///// </summary>
            ///// <param name="totalcount">所有记录数</param>
            ///// <param name="dto">当前实体</param>
            ///// <param name="whereExpression">条件表达式</param>
            ///// <param name="orderByExpression">排序字段表达式</param>
            ///// <param name="ascending">升序排列</param>
            ///// <returns>DepartmentDTO</returns>
            //public static Person FindPrevious(out int totalCount, Department dto, Expression<Func<Person, bool>> whereExpression,
            //                                  Expression<Func<Person, object>> orderByExpression, bool ascending)
            //{

            //    var repo = RepositoryContext.GetRepository();
            //    var list = repo.GetAll(whereExpression, orderByExpression, ascending);
            //    totalCount = list.Count;
            //    int i = 0;
            //    foreach (Person person in list)
            //    {
            //        if (person.ID == dto.ID)
            //        {
            //            break;
            //        }
            //        i = i + 1;
            //    }
            //    if (i == 0)
            //        return null;
            //    return list[i];

            //}

            #endregion
        }
        #endregion
    }
}
