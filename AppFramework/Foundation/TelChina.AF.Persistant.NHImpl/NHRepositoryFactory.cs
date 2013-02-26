using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using TelChina.AF.Sys.Configuration;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Persistant.NHImpl
{
    public class NHRepositoryFactory : RepositoryFactory
    {
       #region 私有变量
        /// <summary>
        /// 缓存sessionFactory, key为appName， value为sessionFacory对象
        /// </summary>
        private static Dictionary<string, ISessionFactory> sessionFactoryCache ;
        
        /// <summary>
        /// 运行时实体映射文件目录
        /// </summary>
        private const string CONFIGPATH = @"Config";

        /// <summary>
        /// modified by zqy
        /// 映射文件目录应该分开
        /// 实体映射文件目录
        /// </summary>
        private const string MAPPINGPATH = @"EntityMapping";
        
        #endregion  
        /// <summary>
        /// 当需要执行无返回值的SQL模板以及存储过程的时候需要按照配置文件返回IDbCommand
        /// </summary>
        public static IDbConnection IDbConnection;

        #region 方法

        /// <summary>
        /// 读取配置文件，根据配置文件中的应用程序 节点，完成sessionFactory的初始化
        /// </summary>
        public override void Config()
        {
            sessionFactoryCache = new Dictionary<string, ISessionFactory>();
            foreach (TelChina.AF.Sys.Configuration.AddElement item in AFConfigurationManager.PLGroup.Storages)
            {
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, item.FileName);
                var config = new NHibernate.Cfg.Configuration();

                var entityListener = new EntityListener();
                config.EventListeners.PostLoadEventListeners = new IPostLoadEventListener[] {entityListener};
                config.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[] {entityListener};
                config.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[] {entityListener};
                config.EventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[] {entityListener};

                config.Configure(fileName);

                //设置实体映射文件关联
                var mappingFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIGPATH, MAPPINGPATH);
                config.AddDirectory(new System.IO.DirectoryInfo(mappingFileName));

                sessionFactoryCache.Add(item.AppName, config.BuildSessionFactory());
                Console.WriteLine(string.Format("{0}对应的sessionfactory已经创建", item.AppName));
            }
            if(sessionFactoryCache.Count <1)
            {
                throw new UnhandledException("NHibernate相关配置不正确");
            }
            //加载ibatis的相关配置，用于动态查询
            DynamicSqlBuilder.InitSqlMapper();
        }

        /// <summary>
        /// 根据传入的应用程序名创建对应APP的仓储对象，由具体实现类 override该方法，返回相应的仓储对象,对于一次Http请求 共享一个仓储对象
        /// 修改：张前园
        /// Time：2012-7-17
        /// 原因：根据需求添加default配置节点
        /// </summary>
        /// <typeparam name="appName"> 应用程序名称</typeparam>
        /// <returns></returns>
        public override IRepository GetRepository(string appName)
        {
             if(sessionFactoryCache.Count >0 && !string.IsNullOrEmpty(appName))
             {
                 if (sessionFactoryCache.ContainsKey(appName))
                 {
                     IDbConnection =
                         ((ISessionFactoryImplementor) sessionFactoryCache[appName]).ConnectionProvider.Driver.
                             CreateConnection();
                     return new NHRepository(sessionFactoryCache[appName].OpenSession());
                 }
                 else
                 {
                     IDbConnection =
                         ((ISessionFactoryImplementor) sessionFactoryCache["default"]).ConnectionProvider.Driver.
                             CreateConnection();         
                     return new NHRepository(sessionFactoryCache["default"].OpenSession());
                 }
             }

             throw new UnhandledException(string.Format("配置出错，无法找到{0} 对应的sessionFactory", appName));
        }

        /// <summary>
        /// 返回IDbCommand,如果为空则表示没有读取配置文件
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetIDbConnection()
        {
            if (IDbConnection != null)
            {
                return IDbConnection;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Author：zqy
        /// Time：2012-5-7
        /// 功能：添加配置文件的字段信息
        /// </summary>
        /// <param name="settingElement"></param>
        public override void AddSettingElement(TelChina.AF.Sys.Configuration.AddElement settingElement)
        {
            AFConfigurationManager.PLGroup.Storages.Add(settingElement);
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(AFConfigurationManager.CONFIGFILENAME));
            XmlDocument xmlDocument = new XmlDocument();            
            xmlDocument.Load(fileName);

            // 将配置文件信息添加到Storages节点下
            XmlNode root = xmlDocument.SelectSingleNode("//storages");
            CreateNode(xmlDocument,root, settingElement);

            xmlDocument.Save(fileName);
        }

        /// <summary>
        /// 创建一个带有属性的XmlNode
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="root">该节点的根节点</param>
        /// <param name="settingElement">节点信息</param>
        private void CreateNode(XmlDocument xmlDocument, XmlNode root, Sys.Configuration.AddElement settingElement)
        {
            XmlNode childNode = xmlDocument.CreateNode(XmlNodeType.Element,"add",null);
            // appName 属性
            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("appName");
            xmlAttribute.Value = settingElement.AppName;
            childNode.Attributes.Append(xmlAttribute);

            // fileName属性
            xmlAttribute = xmlDocument.CreateAttribute("fileName");
            xmlAttribute.Value = settingElement.FileName;
            childNode.Attributes.Append(xmlAttribute);

            root.AppendChild(childNode);
        }
        #endregion
 
    }
}
