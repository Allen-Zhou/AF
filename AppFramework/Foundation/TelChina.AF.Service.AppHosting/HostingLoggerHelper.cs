using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Util.Logging;

namespace TelChina.AF.Service.AppHosting
{
    internal class HostingLoggerHelper
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(() => LogManager.GetLogger("Hosting"));

        public static void Debug(string message)
        {
            Logger.Value.Debug(message);
        }
        public static void Info(string message)
        {
            Logger.Value.Info(message);
        }

        public static void Error(string message)
        {
            Logger.Value.Error(message);
        }
        public static void Error(Exception ex)
        {
            Logger.Value.Error(ex);
        }
    }
}
