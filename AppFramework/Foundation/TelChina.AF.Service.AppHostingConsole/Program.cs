using System;
using TelChina.AF.Service.AppHosting;
using System.Security.Principal;
using TelChina.AF.Persistant;
using TelChina.AF.Util.Logging;


namespace TelChina.AF.Service.AppHostingConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!IsAdministrator())
            {
                Console.WriteLine("应用服务引擎 需要以管理员权限运行...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("按任意键启动服务...");
                try
                {
                    Console.ReadLine();
                    AppHost.Start();
                    RepositoryContext.Config();
                }
                catch (Exception ex)
                {
                    var logger = LogManager.GetLogger("Host");
                    logger.Error(ex);
                }

                Console.ReadLine();
            }
        }
        /// <summary>
        /// 判断是否以管理员权限运行
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            if (identity != null)
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            else
            {
                return false;
            }
        }
    }
}