using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using TelChina.AF.Sys.Configuration;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Persistant
{
    /// <summary>
    /// 仓储工厂的抽象基类，缓存了仓储实例
    /// </summary>
    public abstract class RepositoryFactory
    {
        #region 私有变量
        private static RepositoryFactory _repositoryFactory;

        /// <summary>
        /// 常量，作为存储仓储分发器对象的key
        /// </summary>
        //private const string REPOSIORYDISPATHER_CACHE = "repositoryDispather_cache";

        /// <summary>
        /// 运行时Bin目录
        /// </summary>
        private const string BINPATH = "Bin";

       /* /// <summary>
        /// 仓储对象分发器， 方法级对象 
        /// </summary>
        private RepositoryDispather dispather
        {
         /*   get
            {
                if (CallContext.GetData(REPOSIORYDISPATHER_CACHE) != null)
                {
                    return CallContext.GetData(REPOSIORYDISPATHER_CACHE) as RepositoryDispather;
                }
                var _dispather = new RepositoryDispather(_repositoryFactory);
                CallContext.SetData(REPOSIORYDISPATHER_CACHE, _dispather);
                return _dispather;
            }#1#

            get { return new RepositoryDispather(_repositoryFactory); }

        }*/

        #endregion

        #region 方法

        /// <summary>
        /// 读取配置文件，根据配置文件中的应用程序 节点，完成sessionFactory的初始化
        /// 应用程序启动时 调用
        /// </summary>
        public abstract void Config();

        /// <summary>
        /// 添加配置节点
        /// </summary>
        /// <param name="settingElement"></param>
        public abstract void AddSettingElement(TelChina.AF.Sys.Configuration.AddElement settingElement);

        /// <summary>
        /// 应用程序关闭，清空资源
        /// </summary>
        public virtual void Close()
        {
            _repositoryFactory = null;
        }

        /// <summary>
        /// 反射创建子类的实例
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static RepositoryFactory CreateRepositoryFactory()
        {

            string[] provider = AFConfigurationManager.PLGroup.provider.Split(',');
            if (string.IsNullOrEmpty(provider[0]) || string.IsNullOrEmpty(provider[1]))
            {
                throw new UnhandledException("配置文件Biz.config 内PLGroup Provider属性 配置有误");
            }

            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BINPATH, provider[1].Trim() + ".dll");

            _repositoryFactory =
                (RepositoryFactory)Assembly.LoadFrom(dllPath).CreateInstance(provider[0]);

            return _repositoryFactory;
        }

        /// <summary>
        /// 根据传入的应用程序名创建对应APP的仓储对象，由具体实现类 override该方法，返回相应的仓储对象,对于一次Http请求 共享一个仓储对象
        /// </summary>
        /// <typeparam name="appName"> 应用程序名称</typeparam>
        /// <returns></returns>
        public abstract IRepository GetRepository(string appName);

        /// <summary>
        /// 创建 仓储的分发器 
        /// </summary>
        /// <returns></returns>
        public IRepository GetRepository()
        {
            return new RepositoryDispather(_repositoryFactory);
        }

        #endregion
    }
}
