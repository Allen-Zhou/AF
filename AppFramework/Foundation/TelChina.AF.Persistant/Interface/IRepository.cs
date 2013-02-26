using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace TelChina.AF.Persistant
{
    /// <summary>
    ///  仓储接口，定义了基本行为，对外暴露对实体的CRUD操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepository : IDisposable
    {
        #region 仓储是否关闭

        /// <summary>
        /// 仓储是否已经关闭
        /// </summary>
        /// <returns></returns>
        bool IsClosed();

        #endregion

        #region 增删改 操作
        /// <summary>
        ///  新增一个实体 
        /// </summary>
        /// <param name="item">需要新增到数据库的实体</param>
        void Add(EntityBase item);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="item">需要从数据库删除的实体</param>
        void Remove(EntityBase item);

        /// <summary>
        ///  修改一个实体
        /// </summary>
        /// <param name="item">Item with changes</param>
        void Update(EntityBase item);

        #endregion

        #region 查询操作

        /// <summary>
        /// 根据ID返回对应的记录
        /// </summary>
        /// <returns>List of selected elements</returns>
        TEntity GetByID<TEntity>(Guid ID) where TEntity : EntityBase;
        /// <summary>
        /// 实体查询,返回强类型实体
        /// </summary>
        /// <param name="entitKey">实体Key,包括实体全名和ID</param>
        /// <returns>返回强类型实体,需要类型转换为具体的实体类型</returns>
        EntityBase GetByKey(EntityKey entitKey);

        /// <summary>
        /// 返回数据库中所有TEntity类型的实体
        /// </summary>
        /// <return>结果集</return></returns>
        IList<TEntity> GetAll<TEntity>() where TEntity : EntityBase;

        /// <summary>
        ///  根据给定的条件返回 结果集
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>结果集</returns>
        IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : EntityBase;

        /// <summary>
        ///  根据给定的条件返回 结果集
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序字段表达式</param>
        /// <param name="ascending">升序排列</param>
        /// <returns>结果集</returns>
        IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression,
                                              Expression<Func<TEntity, object>> orderByExpression, bool ascending)
            where TEntity : EntityBase;

        /// <summary>
        ///  根据给定的条件返回 结果集
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序Lamdba表达式</param>
        /// <param name="ascending">升序降序</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">该页行数</param>
        /// <returns>结果集</returns>
        IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending, int pageNumber, int pageSize) where TEntity : EntityBase;

        #region 原生sql 查询

        /// <summary>
        ///  执行查询SQL语句，返回弱类型结果集
        /// </summary>
        /// <param name="statementName">定义的sql模板名称</param>
        /// <param name="parameterObject">参数列表</param>
        /// <returns></returns>
        IList<Hashtable> ExecuteQuery<TEntity>(string statementName, object parameterObject);

        /// <summary>
        ///  执行查询SQL语句，返回对象结果集
        /// </summary>
        /// <param name="statementName">定义的sql模板名称</param>
        /// <param name="parameterObject">参数列表</param>
        /// <returns></returns>
        List<TReturn> ExecuteQuery<TEntity, TReturn>(string statementName, object parameterObject);

        /// <summary>
        /// 执行SQL语句,如果执行成功则返回true，反之执行fasle
        /// </summary>
        /// <param name="statementName">定义的sql模板名称</param>
        /// <param name="parameterObject">参数列表</param>
        bool ExecuteNonQuery<TEntity>(string statementName, object parameterObject);

        /// <summary>
        /// 执行SQL语句返回单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        TReturn ExecuteQueryForDefault<TEntity, TReturn>(string statementName, object parameterObject);

        #endregion

        /// <summary>
        /// 获取泛型IQueryable，用于业务代码组织linq语句
        /// </summary>
        /// <returns> </returns>
        IQueryable<TEntity> Query<TEntity>() where TEntity : EntityBase;
        #endregion

        #region 持久化操作

        /// <summary>
        /// 将实体持久化到数据库中
        /// </summary>
        /// <returns>返回该操作 数据库影响的行数</returns>
        void SaveChanges();

        #endregion

        /// <summary>
        /// 执行存储过程
        /// 返回强类型集合
        /// 备注：NHibernate中无法自动匹配参数
        /// 不可以像SQL模板那样自动匹配参数       
        /// </summary>
        IList<TEntity> ExecuteProcedureForList<TEntity>(string statementName, object parameterObject) where TEntity : EntityBase;

        /// <summary>
        /// 执行存储过程
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <typeparam name="TEntity"> </typeparam>
        /// <param name="statementName">定义的sql模板名称</param>
        /// <param name="parameterObject">参数列表</param>
        /// <returns></returns>
        TReturn ExecuteProcedure<TEntity, TReturn>(string statementName, object parameterObject);

        /// <summary>
        /// 执行无返回值的存储过程
        /// 成功返回true，否则false
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        bool ExecuteProcedureNoResult<TEntity>(string statementName, object parameterObject);

        #region Attach&Detach

        ///// <summary>
        /////  将实体放入仓储，交由仓储托管
        ///// </summary>
        ///// <param name="item">需要托管的实体</param>
        //void Attach(EntityBase item);

        ///// <summary>
        /////  将实体脱离仓储管理
        ///// </summary>
        ///// <param name="item">需要脱离管理的实体</param>
        //void Detach(EntityBase item);

        #endregion

    }
}
