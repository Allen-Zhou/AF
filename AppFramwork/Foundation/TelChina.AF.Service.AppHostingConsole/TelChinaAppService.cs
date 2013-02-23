using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using TelChina.AF.Service.AppHosting;
using TelChina.AF.Persistant;
using TelChina.AF.Util.Logging;

namespace TelChina.AF.Service.AppHostingConsole
{
    partial class TelChinaAppService : ServiceBase
    {
        private const string LOGSOURCE = "TelChinaAppService";
        private const string LOG = "MyNewLog";

        private readonly ILogger _logger = LogManager.GetLogger(LOGSOURCE);
        public TelChinaAppService()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists(LOGSOURCE))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    LOGSOURCE, LOG);
            }
            eventLogger.Source = LOGSOURCE;
            eventLogger.Log = LOG;
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            const string starting = "正在启动系统服务...";
            const string started = "启动系统服务启动完成!";

            WritLog(starting);
            try
            {
                AppHost.Start();
                RepositoryContext.Config();
                WritLog(started);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                eventLogger.WriteEntry(e.Message, EventLogEntryType.Error);
            }
        }

        private void WritLog(string starting)
        {
            _logger.Info(starting);
            eventLogger.WriteEntry(starting, EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            const string stopping = "正在停止系统服务...";
            const string stoped = "系统服务已经停止!";

            WritLog(stopping);
            AppHost.Stop();
            WritLog(stoped);
        }
    }
}
