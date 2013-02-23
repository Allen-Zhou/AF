using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Diagnostics;
using System.IO;


namespace TelChina.AF.Tool.SetupEvent
{
    [RunInstaller(true)]
    public partial class InstallerEvent : System.Configuration.Install.Installer
    {
        public InstallerEvent()
        {
            InitializeComponent();

            base.AfterInstall += new InstallEventHandler(InstallerTelChina_AfterInstall);
        }

        public void InstallerTelChina_AfterInstall(object sender, InstallEventArgs e)
        {
            
            string commonpath =Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.Personal),"安装文件");
            string path = Path.Combine(commonpath , "Doexe.bat");
            bool isExist = File.Exists(path);
            if (!isExist)
            {
                throw new Exception(commonpath + "系统与安装程序要求不一致，请联系平台组！");
            }
            ProcessStartInfo pro = new ProcessStartInfo();
            pro.FileName = path;
            //获取或设置要启动的应用程序或文档
            pro.WorkingDirectory = commonpath;         //获取或设置要启动的进程的初始目录
            pro.Arguments = "Port=5001;EnType=1";
            pro.CreateNoWindow = false;                                     //获取或设置指示是否在新窗口中启动该进程的值
            pro.Verb = "open";                                              //获取或设置打开 FileName 属性指定的应用程序或文档时要使用的谓词
            pro.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; //获取或设置启动进程时使用的窗口状态
            //pro.UseShellExecute = true;                                     //获取或设置一个值，该值指示是否使用操作系统外壳程序启动进程
            Process.Start(pro);
        }

    }
}
