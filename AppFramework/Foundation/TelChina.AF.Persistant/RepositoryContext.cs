using System;

namespace TelChina.AF.Persistant
{
    /// <summary>
    ///  抽象基类，反射加载仓储对象并应用单例模式 对外提供获取仓储的接口
    /// </summary>
    public class RepositoryContext  
    {
        #region 私有变量

        /// <summary>
        ///  仓储工厂基类，全局唯一
        /// </summary>
        private static RepositoryFactory _repositoryFactory;
        #endregion

        #region  方法
 
        /// <summary>
        /// 配置所有的资源
        /// </summary>
        public static void Config()
        {
            Console.WriteLine("正在读取biz.config");
            //读取平台的配置文件
            TelChina.AF.Sys.Configuration.AFConfigurationManager.Setup();
             
            //获取仓储工厂的实例
            _repositoryFactory = RepositoryFactory.CreateRepositoryFactory();
            Console.WriteLine("已经通过反射完成仓储工厂对象的创建");
            //配置仓储工厂
            _repositoryFactory.Config();
            Console.WriteLine("PL相关所有配置已经完成");

            #region Test Add && Remove
            //_repositoryFactory.AddSettingElement(new System.Configuration.SettingElement { AppName = "Demo", FileName = "hibernate1.cfg.xml" });
            #endregion
        }

        /// <summary>
        /// 关闭所有的资源
        /// </summary>
        protected static void Close()
        {
            _repositoryFactory.Close();
            _repositoryFactory = null;
        }

       
        /// <summary>
        /// 获取当前仓储对象，无则创建并放入缓存，对于一次Http请求 共享一个仓储对象
        /// </summary>
        /// <returns></returns>
        public static IRepository GetRepository()
        {
            return _repositoryFactory.GetRepository();
        }

        #endregion

    }
}
