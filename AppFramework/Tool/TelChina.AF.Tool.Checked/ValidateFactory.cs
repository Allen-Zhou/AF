using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;

namespace TelChina.AF.Tool.Checked
{
    public class ValidateFactory
    {
        private static IModelingEvent modelingEvent;


        /// <summary>
        /// 反射创建子类的实例
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static IModelingEvent GetIModelingEvent()
        {
            if (modelingEvent == null)
            {
                EnvDTE80.DTE2 dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

                string version = SerachFile(dte.Solution.FullName, false);

                var dllPath = Path.Combine("D:\\TelChina\\PlatformModule", version, "Build\\Platform", "TelChina.AF.Tool.CheckImpl.dll");
                //不存在组件，
                if (!File.Exists(dllPath))
                {
                    return modelingEvent;
                }
                var typies = Assembly.LoadFrom(dllPath).GetExportedTypes();

                foreach (Type type in typies)
                {
                    object classEvent = Assembly.LoadFrom(dllPath).CreateInstance(type.FullName);
                    //如果存在多个 只取取得的第一个，一个版本只能有1个
                    if (classEvent is IModelingEvent)
                    {
                        modelingEvent = (IModelingEvent)classEvent;
                        break;
                    }
                }
            }
            return modelingEvent;
        }
        /// <summary>
        /// 通过文件夹路径找到平台版本信息
        /// </summary>
        /// <param name="fileDirectory"></param>
        /// <param name="isFolder"></param>
        /// <returns></returns>
        public static string SerachFile(string fileDirectory, bool isFolder)
        {
            if (!isFolder)
            {
                var paths = fileDirectory.Split('\\');
                string filePath = string.Empty;
                for (int i = 0; i < paths.Count() - 1; i++)
                {
                    filePath += paths[i];
                    if (i < paths.Count() - 2)
                    {
                        filePath += "\\";
                    }
                }
                fileDirectory = filePath;
            }
            DirectoryInfo dir = new DirectoryInfo(fileDirectory);
            FileSystemInfo[] f = dir.GetFileSystemInfos();//获取文件夹下文件
            string version = string.Empty;
            foreach (FileSystemInfo i in f)
            {
                if (i is DirectoryInfo)
                {
                    version = SerachFile(i.FullName, true);//递归调用
                    if (!string.IsNullOrEmpty(version))
                        break;
                }
                else
                {
                    if (i.Name.Contains(".modelproj"))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(i.FullName);
                        foreach (XmlNode projectNode in xmlDoc.ChildNodes)
                        {
                            version = FindXmlNode(projectNode);
                            if (!string.IsNullOrEmpty(version))
                                break;
                        }
                    }
                }
            }
            return version;
        }

        /// <summary>
        /// 通过xml找到其中的版本信息
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string FindXmlNode(XmlNode xmlNode)
        {
            string version = string.Empty;
            if (xmlNode.Name == "ArchitectureToolsVersion")
            {
                version = xmlNode.Value;
            }
            else
            {
                if (xmlNode.HasChildNodes)
                {
                    foreach (XmlNode xn in xmlNode.ChildNodes)
                    {
                        if (xn.Name == "ArchitectureToolsVersion")
                        {
                            version = xn.InnerText;
                            break;
                        }
                        else
                        {
                            version = FindXmlNode(xn);
                            if (!string.IsNullOrEmpty(version))
                                break;
                        }
                    }
                }
            }

            return version;
        }

    }
}
