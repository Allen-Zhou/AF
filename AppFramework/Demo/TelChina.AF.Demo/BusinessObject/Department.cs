using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;
using System.Collections;
using System.Linq.Expressions;

namespace TelChina.AF.Demo
{
    public partial class Department : EntityBase
    {
        public Department()
        {
            this._persons.ItemChanged += this.OnPersonSetItemChanged;
            this._childrenDepartment.ItemChanged += this.OnChildrenSetItemChanged;
        }
        protected override void OnDeleted()
        {

        }

        protected override void OnDeleting()
        {

        }

        protected override void OnInserted()
        {

        }

        protected override void OnInserting()
        {
            //关于InId的修改
            //string inid = "";

            //if (this.Parent != null)
            //{
            //    Department parentDTO = Finder.FindById(Parent.ID);
            //    if (parentDTO != null)
            //    {
            //        inid = parentDTO.InId.ToString();
            //    }
            //}

            //if (inid != "")
            //{
            //    inid += "_" + this.ID.ToString();
            //}
            //else
            //{
            //    inid = this.ID.ToString();
            //}

            //this.InId = inid;
        }

        protected override void OnUpdated()
        {

        }

        protected override void OnUpdating()
        {

        }

        protected override void OnValidate()
        {
            //部门编码必须输入检查
            if (this.Code == null || this.Code.Trim().Length == 0)
            {
                throw new Exception("部门编码不能为空");
            }

            if (this.Code.Trim().Length > 30)
            {
                throw new Exception("部门编码应该多于30位");
            }

            //部门名称必须输入检查
            if (this.Name == null || this.Name.Trim().Length == 0)
            {
                throw new Exception("部门名称不能为空");
            }

            if (this.Name.Trim().Length > 50)
            {
                throw new Exception("部门名称不能多于50位");
            }

            if (this.Code.Trim().Contains(";"))
            {
                //编码中不能出现半角分号
                throw new Exception("部门编码中不能出现半角分号");
            }
            if (this.Code.Trim().Contains("；"))
            {
                //编码中不能出现全角分号
                throw new Exception("部门编码中不能出现全角分号");
            }

            /// 注释原因：业务需要

            //上级部门不能是停用部门
            //if (this.Parent != null)
            //{
            //    Department departmentDto = Finder.FindById(this.Parent.ID);
            //    if (departmentDto.Disabled)
            //    {
            //        throw new Exception("上级部门不能是停用部门");
            //    }
            //}
        }

        protected override void SetDefaultValue()
        {
            if (string.IsNullOrEmpty(this.Code))
            {
                this.Code = "Department-001";
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                this.Name = "综合部";
            }

            this.IsEndNode = true;

            if (this.Parent != null)
            {
                this.Depth = this.Parent.Depth + 1;
            }
            else
            {
                this.Depth = 1;
            }
        }

        private EventHandler<ItemChangedEventArgs<Person>> _SetItemChangedHandler; 

        protected override void ChangeSet(Iesi.Collections.Generic.ISet<EntityBase> iSet)
        {
            if (iSet == null)
            {
                return;
            }
            if (this._SetItemChangedHandler == null)
            {
                this._SetItemChangedHandler = this.OnPersonSetItemChanged;
            }

            if (this._persons != null)
            {
                this._persons.ItemChanged -= this._personSetItemChangedHandler;
            }

            //foreach (var person in iSet)
            //{
            //    this._persons.Add(person);
            //}

            this._persons.ItemChanged += this._personSetItemChangedHandler;
        }

        #region Finder类
        public class Finder
        {
            #region 公开方法
            /// <summary>
            /// 通过条件查询 返回IList
            /// </summary>
            /// <returns>IList</returns>
            public static IList<Department> FindList()
            {
                var repo = RepositoryContext.GetRepository();
                IList<Department> resultList;
                resultList = repo.GetAll<Department>();

                return resultList;
            }

            /// <summary>
            /// 通过条件查询 返回IList
            /// </summary>
            /// <param name="whereExpression">条件表达式</param>
            /// <param name="orderByExpression">排序字段表达式</param>
            /// <param name="ascending">升序排列</param>
            /// <returns>IList</returns>
            public static IList<Department> FindList(Expression<Func<Department, bool>> whereExpression,
                                           Expression<Func<Department, object>> orderByExpression, bool ascending)
            {
                var repo = RepositoryContext.GetRepository();
                IList<Department> resultList;
                resultList = repo.GetAll<Department>(whereExpression, orderByExpression, ascending);
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
            public static IList<Department> FindList(int pageIndex, int pageCount, Expression<Func<Department, bool>> whereExpression,
                                              Expression<Func<Department, object>> orderByExpression, bool ascending)
            {
                var repo = RepositoryContext.GetRepository();
                IList<Department> resultList;
                resultList = repo.GetAll<Department>(whereExpression, orderByExpression, ascending, pageIndex, pageCount);
                return resultList;
            }


            /// <summary>
            /// 通过条件查询 返回Department
            /// </summary>
            /// <param name="id">Guid</param>
            /// <returns>Department</returns>
            public static Department FindById(Guid id)
            {
                if (id == null)
                {
                    return null;
                }
                var repo = RepositoryContext.GetRepository();
                Department resultDto = repo.GetByID<Department>(id);

                return resultDto;
            }

            /// <summary>
            /// 通过条件查询 返回首张
            /// </summary>
            /// <param name="totalcount">所有记录数</param>
            /// <param name="whereExpression">条件表达式</param>
            /// <param name="orderByExpression">排序字段表达式</param>
            /// <param name="ascending">升序排列</param>
            /// <returns>DepartmentDTO</returns>
            public static Department FindFirst(out int totalCount, Expression<Func<Department, bool>> whereExpression,
                                              Expression<Func<Department, object>> orderByExpression, bool ascending)
            {
                var repo = RepositoryContext.GetRepository();
                var list = repo.GetAll(whereExpression, orderByExpression, ascending);
                totalCount = list.Count;
                return list.First();

            }

            /// <summary>
            /// 通过条件查询 返回最后一张
            /// </summary>
            /// <param name="totalcount">所有记录数</param>
            /// <param name="whereExpression">条件表达式</param>
            /// <param name="orderByExpression">排序字段表达式</param>
            /// <param name="ascending">升序排列</param>
            /// <returns>Department</returns>
            public static Department FindLast(out int totalCount, Expression<Func<Department, bool>> whereExpression,
                                              Expression<Func<Department, object>> orderByExpression, bool ascending)
            {
                var repo = RepositoryContext.GetRepository();
                var list = repo.GetAll(whereExpression, orderByExpression, ascending);
                totalCount = list.Count;
                return list.Last();
            }

            /// <summary>
            /// 通过条件查询 返回下一张
            /// </summary>
            /// <param name="totalcount">所有记录数</param>
            /// <param name="dto">当前实体</param>
            /// <param name="whereExpression">条件表达式</param>
            /// <param name="orderByExpression">排序字段表达式</param>
            /// <param name="ascending">升序排列</param>
            /// <returns>DepartmentDTO</returns>
            public static Department FindNext(out int totalCount, Department dto, Expression<Func<Department, bool>> whereExpression,
                                              Expression<Func<Department, object>> orderByExpression, bool ascending)
            {

                var repo = RepositoryContext.GetRepository();
                var list = repo.GetAll(whereExpression, orderByExpression, ascending);
                totalCount = list.Count;
                int i = 0;
                foreach (Department department in list)
                {
                    i = i + 1;
                    if (department.ID == dto.ID)
                    {
                        break;
                    }
                }
                if (i == totalCount)
                    return null;
                return list[i];

            }

            /// <summary>
            /// 通过条件查询 返回上一张
            /// </summary>
            /// <param name="totalcount">所有记录数</param>
            /// <param name="dto">当前实体</param>
            /// <param name="whereExpression">条件表达式</param>
            /// <param name="orderByExpression">排序字段表达式</param>
            /// <param name="ascending">升序排列</param>
            /// <returns>DepartmentDTO</returns>
            public static Department FindPrevious(out int totalCount, Department dto, Expression<Func<Department, bool>> whereExpression,
                                              Expression<Func<Department, object>> orderByExpression, bool ascending)
            {

                var repo = RepositoryContext.GetRepository();
                var list = repo.GetAll(whereExpression, orderByExpression, ascending);
                totalCount = list.Count;
                int i = 0;
                foreach (Department department in list)
                {
                    if (department.ID == dto.ID)
                    {
                        break;
                    }
                    i = i + 1;
                }
                if (i == 0)
                    return null;
                return list[i];

            }

            #endregion
        }
        #endregion





    }
}
