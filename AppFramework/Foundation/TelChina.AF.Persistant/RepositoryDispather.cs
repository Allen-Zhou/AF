using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Persistant
{
    /// <summary>
    /// 仓储对象协调者，负责仓储的协调和管理，根据应用名称调用对应的仓储对象
    /// </summary>
    public class RepositoryDispather : IRepository
    {
        #region 私有变量

        //private const string REPOSITORIES_CACHES = "RepositoriesCaches";

        /// <summary>
        /// 缓存仓储实例
        /// 一个推荐的做法是在一次http请求中只操作一个数据库，即只生成一个仓储对象
        /// </summary>
        private Dictionary<string, IRepository> repositories
        {
            set;
            get;
            /*get { return CallContext.GetData(REPOSITORIES_CACHES) as Dictionary<string, IRepository>; }
            set { CallContext.SetData(REPOSITORIES_CACHES, value); }*/
        }

        /// <summary>
        /// 私有成员，抽象仓储工厂对象引用，指向具体实现的类的实例
        /// </summary>
        private readonly RepositoryFactory _repositoryFactory;

        #endregion

        #region 含参构造函数


        /// <summary>
        /// 构造函数，持有仓储工厂的引用，引用指向具体仓储工厂子类的实例
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public RepositoryDispather(RepositoryFactory repositoryFactory)
        {
            this._repositoryFactory = repositoryFactory;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 释放线程内的资源
        /// </summary>
        public void Dispose()
        {
            //释放线程级缓存
            if (repositories != null)
            {
                foreach (var key in repositories.Keys)
                {
                    repositories[key].Dispose();
                }
                repositories.Clear();

                /*//清空repository缓存
                repositories.Clear();

                if (repositories.Count == 0)
                {
                    //释放缓存
                    CallContext.FreeNamedDataSlot(REPOSITORIES_CACHES);
                }*/
            }
        }

        /// <summary>
        /// 遍历当前所有的仓储对象，当且仅当 所有的仓储都已经close，则返回true
        /// </summary>
        /// <returns></returns>
        public bool IsClosed()
        {
            return repositories.Values.All(repo => repo.IsClosed());
        }

        /// <summary>
        /// 根据参数类型 匹配正确的仓储进行新增操作
        /// </summary>
        /// <param name="item"></param>
        public void Add(EntityBase item)
        {
            GetConcreteRepository(GetAppName(item.GetType())).Add(item);
        }

        /// <summary>
        /// 根据参数类型 匹配正确的仓储进行删除操作
        /// </summary>
        /// <param name="item"></param>
        public void Remove(EntityBase item)
        {
            GetConcreteRepository(GetAppName(item.GetType())).Remove(item);
        }

        /// <summary>
        /// 根据参数类型 匹配正确的仓储进行修改操作
        /// </summary>
        /// <param name="item"></param>
        public void Update(EntityBase item)
        {
            GetConcreteRepository(GetAppName(item.GetType())).Update(item);
        }

        /// <summary>
        /// 根据泛型类型 匹配正确的仓储 按照ID查询实体
        /// </summary>
        /// <param name="ID">实体的ID</param>
        public TEntity GetByID<TEntity>(Guid ID) where TEntity : EntityBase
        {
            return GetConcreteRepository(GetAppName<TEntity>()).GetByID<TEntity>(ID);
        }

      
        /// <summary>
        /// 实体查询,返回强类型实体
        /// </summary>
        /// <param name="entitKey">实体Key,包括实体全名和ID</param>
        /// <returns>返回强类型实体,需要类型转换为具体的实体类型</returns>
        public EntityBase GetByKey(EntityKey entitKey)
        {
            if (entitKey == null || entitKey.IsEmpty)
            {
                return null;
            }
            return GetConcreteRepository(GetAppName(entitKey.EntityType)).GetByKey(entitKey);
        }
        /// <summary>
        /// 根据泛型类型 匹配正确的仓储 查询全部该类型实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns></returns>
        public IList<TEntity> GetAll<TEntity>() where TEntity : EntityBase
        {
            return GetConcreteRepository(GetAppName<TEntity>()).GetAll<TEntity>();
        }

        /// <summary>
        /// 根据泛型类型 匹配正确的仓储 查询符合条件的该类型实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns></returns>
        public IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression)
            where TEntity : EntityBase
        {
            return GetConcreteRepository(GetAppName<TEntity>()).GetAll<TEntity>(whereExpression);
        }

        /// <summary>
        /// 根据泛型类型 匹配正确的仓储 查询符合条件的该类型实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序字段</param>
        /// <param name="ascending">排序方向</param>
        /// <returns></returns>
        public IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression,
                                              Expression<Func<TEntity, object>> orderByExpression, bool ascending)
            where TEntity : EntityBase
        {
            return GetConcreteRepository(GetAppName<TEntity>()).GetAll<TEntity>(whereExpression, orderByExpression, ascending);
        }

        /// <summary>
        /// 根据泛型类型 匹配正确的仓储 查询符合条件的该类型实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序字段</param>
        /// <param name="ascending">排序方向</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">每页记录条数</param>
        /// <returns></returns>
        public IList<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> whereExpression,
                                              Expression<Func<TEntity, object>> orderByExpression, bool ascending,
                                              int pageNumber, int pageSize) where TEntity : EntityBase
        {
            return GetConcreteRepository(GetAppName<TEntity>()).GetAll<TEntity>(whereExpression, orderByExpression, ascending, pageNumber, pageSize);
        }

        public IList<Hashtable> ExecuteQuery<TEntity>(string statementName, object parameterObject)
        {
            return GetConcreteRepository(GetAppName<TEntity>()).ExecuteQuery<TEntity>(statementName, parameterObject);
        }

        public bool ExecuteNonQuery<TEntity>(string statementName, object parameterObject)
        {
            return GetConcreteRepository(GetAppName<TEntity>()).ExecuteNonQuery<TEntity>(statementName, parameterObject);
        }

        public TReturn ExecuteQueryForDefault<TEntity, TReturn>(string statementName, object parameterObject)
        {
            return GetConcreteRepository(GetAppName<TEntity>()).ExecuteQueryForDefault<TEntity, TReturn>(statementName, parameterObject);
        }
        /// <summary>
        ///  执行查询SQL语句，返回弱类型结果集
        /// </summary>
        /// <param name="statementName">定义的sql模板名称</param>
        /// <param name="parameterObject">参数列表</param>
        /// <returns></returns>
        public List<TReturn> ExecuteQuery<TEntity, TReturn>(string statementName, object parameterObject)
        {
            return GetConcreteRepository(GetAppName<TEntity>()).ExecuteQuery<TEntity, TReturn>(statementName, parameterObject);
        }


        /// <summary>
        /// 返回泛型IQueryable对象，用于组织linq查询语句 
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : EntityBase
        {
            return GetConcreteRepository(GetAppName<TEntity>()).Query<TEntity>();
        }


        /// <summary>
        /// 将仓储托管的对象持久化到数据库中，并同步数据库和对象的实体状态
        /// </summary>
        public void SaveChanges()
        {
            if (repositories == null)
            {
                return;                
            }
            foreach (var key in repositories.Keys)
            {
                repositories[key].SaveChanges();
            }
        }

        /// <summary>
        /// 根据类型获取应用名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private string GetAppName(Type type)
        {
            string appName;
            if (string.IsNullOrEmpty(type.Namespace))
                appName = type.BaseType.Namespace;
            else
                appName = type.Namespace;
            return GetAppName(appName);
        }

        /// <summary>
        /// 根据泛型参数获取应用名
        /// </summary>
        /// <typeparam name="TEntity">泛型参数</typeparam>
        /// <returns></returns>
        private string GetAppName<TEntity>()
        {
            if (!String.IsNullOrEmpty(typeof(TEntity).Namespace))
            {
                return GetAppName(typeof(TEntity).Namespace);
            }
            else
            {
                return GetAppName(typeof(TEntity).BaseType.Namespace);
            }
        }

        /// <summary>
        /// 根据命名空间获取应用名
        /// </summary>
        /// <param name="strnNameSpace"></param>
        /// <returns></returns>
        /// <exception cref="InvalidConfigurationException"></exception>
        private string GetAppName(string strnNameSpace)
        {
            if (!string.IsNullOrEmpty(strnNameSpace))
            {
                var names = strnNameSpace.Split('.');
                if (names.Length >= 3)
                    return names[2];
            }
            throw new UnhandledException("命名空间出错");
        }

        /// <summary>
        /// 根据应用程序名 获取对应的仓储对象
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        private IRepository GetConcreteRepository(string appName)
        {
            IRepository _repository;
            //如果仓储字典表里没有仓储实例，则初始化仓储字典表，并且通过工厂创建一个仓储放入字典表缓存起来
            if (repositories == null)
            {
                repositories = new Dictionary<string, IRepository>();
                _repository = this._repositoryFactory.GetRepository(appName);
                repositories.Add(appName, _repository);
            }
            else
            {
                //如果该缓存的字典表中没有该仓储，则 根据appName创建一个仓储实例并放入字典缓存
                if (!repositories.ContainsKey(appName))
                {
                    _repository = this._repositoryFactory.GetRepository(appName);
                    repositories.Add(appName, _repository);
                }
                else
                {
                    //如果缓存的字典中有对应的仓储 但已经关闭了，则从字典中删除该仓储对象，根据appName新建一个存入该字典
                    if (repositories[appName].IsClosed())
                    {
                        repositories.Remove(appName);
                        _repository = this._repositoryFactory.GetRepository(appName);
                        repositories.Add(appName, _repository);
                    }
                    else
                    {
                        _repository = repositories[appName];
                    }
                }
            }
            return _repository;
        }


        public TReturn ExecuteProcedure<TEntity, TReturn>(string statementName, object parameterObject)
        {
            return GetConcreteRepository(GetAppName<TEntity>()).ExecuteProcedure<TEntity, TReturn>(statementName, parameterObject);
        }

        public IList<TEntity> ExecuteProcedureForList<TEntity>(string statementName, object parameterObject) where TEntity : EntityBase
        {
            return GetConcreteRepository(GetAppName<TEntity>()).ExecuteProcedureForList<TEntity>(statementName, parameterObject);
        }

        public bool ExecuteProcedureNoResult<TEntity>(string statementName, object parameterObject)
        {
            return GetConcreteRepository(GetAppName<TEntity>()).ExecuteProcedureNoResult<TEntity>(statementName, parameterObject);
        }

        #endregion
        
    }
}
