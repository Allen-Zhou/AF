using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using TelChina.AF.Util.Logging;
using TelChina.AF.Util.Common;
using System.IO;

namespace TelChina.AF.Service.AppHosting
{
    /// <summary>
    /// 职责:装配服务
    /// </summary>
    public class AddinHost
    {
        static CompositionContainer container;
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        static bool isInit;
        /// <summary>
        /// 装配服务
        /// </summary>
        public void Compose(object Importer)
        {
            if (!isInit)
            {
                container = Init();
                isInit = true;
            }
            //将部件（part）和宿主程序添加到组合容器
            container.ComposeParts(Importer);
            //container.ComposeParts(this, new ComputerBookService());
        }

        private static CompositionContainer Init()
        {
            HostingLoggerHelper.Debug("AddinHost.Init started");

            DirectoryCatalog catalog = null;
            var aggregateCatalog = new AggregateCatalog();

            if (ConfigHelper.GetConfigValue("AddinPath") != null
                && Directory.Exists(Directory.GetCurrentDirectory() +
                                                "\\" + ConfigHelper.GetConfigValue("AddinPath"))
                )
            {
                var path = Directory.GetCurrentDirectory() +
                                 "\\" + ConfigHelper.GetConfigValue("AddinPath");
                catalog = new DirectoryCatalog(path);
                HostingLoggerHelper.Debug("AddinPath:" + path);
                aggregateCatalog.Catalogs.Add(catalog);
            }

            string currentpath = @".\";
            var currentcatalog = new DirectoryCatalog(currentpath);
            aggregateCatalog.Catalogs.Add(currentcatalog);

            var container = new CompositionContainer(aggregateCatalog);
            HostingLoggerHelper.Debug("AddinHost.Init finished");
            return container;
        }
    }
}
