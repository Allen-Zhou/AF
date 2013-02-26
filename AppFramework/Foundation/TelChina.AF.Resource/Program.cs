using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TelChina.AF.Sys.Configuration;

namespace TelChina.AF.Resource
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AFConfigurationManager.Setup();
            Application.Run(new Resource());
        }
    }
}
