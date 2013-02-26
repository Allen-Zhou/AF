using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Reflection;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Linq;
using NHibernate.Transform;
using TelChina.AF.Persistant.Exceptions;
using TelChina.AF.Sys.Exceptions;
using TelChina.AF.Util.Logging;


namespace TelChina.AF.Persistant.NHImpl
{
    /// <summary>
    /// 基于NHibernate的仓储实现类，实现了仓储接口定义的CRUD操作
    /// </summary>
    public class NHRepository : IRepository
    {
        private ILogger logger = LogManager.GetLogger("PL");
        #region 私有成员
        /// <summary>
        /// NHibernate的session对象
        /// </summary>
        private ISession session;
        #endregion

        #region 构造器

        /// <summary>
        /// 通过外部传入Session的构造基于NH的仓储对象
        /// </summary>
        /// <param name="session"></param>
        internal NHRepository(ISession session)
        {
            this.session = session;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断仓储是否关闭
        /// </summary>
        /// <returns></returns>
        public bool IsClosed()
        {
            return !this.session.IsOpen;
        }

        /// <summary>
        ///  新增一个实体 
        /// </summary>
        /// <param name="item">需要新增到数据库的实体</param>
        public void Add(EntityBase item)
        {
            if (item == null)
                throw new UnhandledException("参数不得为空");

            //TODO:需要检查实体状态，是否允许放入集合，如Unchanged状态是否能放入新增集合
            if (item.SysState == EntityStateEnum.Unchanged && !session.Contains(item))
            {
                var persistableObject = item as IPersistableObject;
                persistableObject.SysState = EntityStateEnum.Inserting;
                persistableObject.SetDefaultValue();
                persistableObject.OnValidate();
                persistableObject.OnInserting();
                try
                {
                    session.Save(item);
                }
                catch (NonUniqueObjectException e)
                {
                    throw new NonUniqueEntityException(string.Format("ID为{0}对象已经被持久化", item.ID)) { InnerException = e };
                }
            }
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="item">待删除的实体</param>
        public void Remove(EntityBase item)
        {
            if (item == null)
                throw new ArgumentNullException("item", Messages.exception_ItemArgumentIsNull);

            //deleted状态不能做此操作
            if (item.SysState == EntityStateEnum.Deleted || item.SysState == EntityStateEnum.Deleting)
            {
                throw new ConcurrentModificationException(string.Format("ID为{0}的{1}实体已经被删除!", item.ID, item.GetType().ToString()));
            }

            if (
                (item.SysState != EntityStateEnum.Updating && item.SysState != EntityStateEnum.Unchanged &&
                 item.SysState != EntityStateEnum.Inserting))
                return;

            var persistableObject = item as IPersistableObject;
            persistableObject.SysState = EntityStateEnum.Deleting;
            persistableObject.OnDeleting();
            //删除该对象
            session.Delete(item);

        }

        /// <summary>
        ///  修改一个实体
        /// </summary>
        /// <param name="item">Item with changes</param>
        public void Update(EntityBase item)
        {
            if (item == null)
                throw new UnhandledException("更新操作的参数不能为空");

            if ((item.SysState == EntityStateEnum.Unchanged || item.SysState == EntityStateEnum.Updating))
            {
                var persistableObject = item as IPersistableObject;
                persistableObject.SysState = EntityStateEnum.Updating;
                persistableObject.SetDefaultValue();
                persistableObject.OnValidate();
                persistableObject.OnUpdating();
                session.Update(item);
            }
            else if (item.SysState == EntityStateEnum.Deleted || item.SysState == EntityStateEnum.Deleting)
            {
                throw new ConcurrentModificationException(string.Format("ID为{0}的{1}实体已经被删除!", item.ID, item.GetType().ToString()));
            }
        }

        /// <summary>
        /// 根据ID返回对应的记录
        /// </summary>
        /// <returns>List of selected elements</returns>
        public TEntity GetByID<TEntity>(Guid ID) where TEntity : EntityBase
        {
            return session.Get<TEntity>(ID);
            //return (from item in session.Query<TEntity>() where item.ID == ID select item).FirstOrDefault();
        }

        /// <summary>
        /// 获取泛型IQueryable，用于业务代码组织linq语句
        /// </summary>
        /// <returns> </returns>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : EntityBase
        {
            return session.Query<TEntity>();
        }

        /// <summary>
        /// 返回数据库中所有TEntity类型的实体
        /// </summary>
        /// <returns>List of selected elements</returns>
        public IList<TEntity> GetAll<TEntity>() where TEntity : EntityBase
        {
            return session.CreateCriteria(typeof(TEntity)).List<TEntity>();
        }

        /// <summary>
        ///  根据给定的条件返回 结果集
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>List of selected elements</returns>
        public IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : EntityBase
        {
            return GetAll<TEntity>(whereExpression, null, false, -1, -1);
        }

        /// <summary>
        ///  根据给定的条件返回 结果集
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序Lamdba表达式</param>
        /// <param name="ascending">升序降序</param>
        /// <returns>List of selected elements</returns>
        public IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending) where TEntity : EntityBase
        {
            return GetAll<TEntity>(whereExpression, orderByExpression, ascending, -1, -1);
        }

        /// <summary>
        ///  根据给定的条件返回 结果集
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序Lamdba表达式</param>
        /// <param name="ascending">升序降序</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">该页行数</param>
        /// <returns>程序集</returns>
        public IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending, int pageNumber, int pageSize) where TEntity : EntityBase
        {
            var iqueryable = session.QueryOver<TEntity>().Where(whereExpression);
            if (orderByExpression != null)
            {
                iqueryable = ascending
                                 ? iqueryable.OrderBy(orderByExpression).Asc
                                 : iqueryable.OrderBy(orderByExpression).Desc;
            }

            if (pageNumber >= 0 && pageSize > 0)
            {
                return iqueryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).List<TEntity>();
            }
            else
            {
                return iqueryable.List<TEntity>();
            }
        }

        /// <summary>
        ///  执行查询SQL语句，返回弱类型结果集
        /// </summary>
        /// <param name="statementName">定义的sql模板名称</param>
        /// <param name="parameterObject">参数列表</param>
        /// <returns></returns>
        public IList<Hashtable> ExecuteQuery<TEntity>(string statementName, object parameterObject)
        {

            if (string.IsNullOrEmpty(statementName))
                throw new UnhandledException("sql模版名称不能为空");
            try
            {
                string sql = DynamicSqlBuilder.GetSql(statementName, parameterObject);
                sql = sql.Trim(',');    // 在存储过程传参中以防出现最后又逗号后缀的问题
                return session.CreateSQLQuery(sql).SetResultTransformer(Transformers.AliasToEntityMap).List<Hashtable>();
            }
            catch (Exception e)
            {
                logger.Error(e);
                return null;
            }
        }

        /// <summary>
        ///  执行查询SQL语句，返回强类型结果集
        /// </summary>
        /// <param name="statementName">定义的sql模板名称</param>
        /// <param name="parameterObject">参数列表</param>
        /// <returns></returns>
        public List<TReturn> ExecuteQuery<TEntity, TReturn>(string statementName, object parameterObject)
        {
            var t = ExecuteQuery<TEntity>(statementName, parameterObject);

            return ReflectorResult<TReturn>(t);
        }

        /// <summary>
        /// 反射创建对象，将hashtable 映射为给定的类型
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private List<TReturn> ReflectorResult<TReturn>(IList<Hashtable> t)
        {
            List<TReturn> result = new List<TReturn>();
            foreach (Hashtable item in t)
            {
                var record = Activator.CreateInstance<TReturn>();
                foreach (var col in item.Keys)
                {
                    var recordProperty = record.GetType().GetProperty(col.ToString(),
                        //如果没有BindingFlags.Instance 方法没有返回值
                        BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
                    if (recordProperty == null)
                    {
                        //TODO 没有找到匹配的属性
                        //throw new Exception();
                        logger.Warn(string.Format("SQL模板查询返回信息对象化过程中,列{0}没有找到匹配的属性", col.ToString()));
                        continue;
                    }
                    recordProperty.SetValue(record, item[col], null);
                }
                result.Add(record);
            }
            return result;
        }

        /// <summary>
        /// 执行无返回值的SQL语句
        /// </summary>
        /// <param name="statementName">SQL模板</param>
        /// <param name="parameterObject">参数列表</param>
        /// <returns></returns>
        public bool ExecuteNonQuery<TEntity>(string statementName, object parameterObject)
        {
            if (string.IsNullOrEmpty(statementName))
                throw new UnhandledException("sql模版名称不能为空");

            return PExecuteNonQuery(statementName, parameterObject);
        }

        private bool PExecuteNonQuery(string statementName, object parameterObject)
        {
            try
            {
                string sql = DynamicSqlBuilder.GetSql(statementName, parameterObject); // 拼接sql语句
                sql = sql.Trim(','); // 在存储过程传参中以防出现最后又逗号后缀的问题            
                // NHibernate提供的方法，可以不关心具体数据库类型
                using (IDbConnection conn = NHRepositoryFactory.GetIDbConnection())
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    IDbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                return true;
                //session.CreateSQLQuery(sql).UniqueResult();     // 执行SQL语句
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }

        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        public TReturn ExecuteQueryForDefault<TEntity, TReturn>(string statementName, object parameterObject)
        {
            if (string.IsNullOrEmpty(statementName))
                throw new UnhandledException("sql模版名称不能为空");

            string sql = DynamicSqlBuilder.GetSql(statementName, parameterObject); // 拼接sql语句
            sql = sql.Trim(','); // 在存储过程传参中以防出现最后又逗号后缀的问题           
            return session.CreateSQLQuery(sql).UniqueResult<TReturn>(); // 执行SQL语句 如果没有值就会返回Null是由API决定的
        }

        /// <summary>
        /// 提交更新，将集合的数据持久化到数据库中
        /// </summary>
        /// <returns>更新的数据行数</returns>
        public void SaveChanges()
        {
            try
            {
                session.Flush();
            }
            catch (ADOException e)
            {
                throw new ConcurrentModificationException("数据保存失败", e);
            }
            catch (NonUniqueObjectException e)
            {
                throw new NonUniqueEntityException("主键冲突", e);
            }
            catch (HibernateException e)
            {
                throw new BusinessException("数据保存失败", e);
            }
        }

        /// <summary>
        /// 释放session持有的资源
        /// </summary>
        public void Dispose()
        {
            //只有在session负责的所有事务都结束时才真正释放
            this.session.Dispose();
        }

        public TReturn ExecuteProcedure<TEntity, TReturn>(string statementName, object parameterObject)
        {
            return GetSqlMapper().QueryForObject<TReturn>(statementName, parameterObject);
        }

        public IList<TEntity> ExecuteProcedureForList<TEntity>(string statementName, object parameterObject) where TEntity : EntityBase
        {
            return GetSqlMapper().QueryForList<TEntity>(statementName, parameterObject);
        }

        public bool ExecuteProcedure<TEntity>(string statementName, object parameterObject)
        {
            throw new NotImplementedException();
        }

        private ISqlMapper GetSqlMapper()
        {
            var builder = new DomSqlMapBuilder();
            var configfileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "SqlMap.config");
            return builder.Configure(configfileName);
        }

        #endregion


        
    }
}
