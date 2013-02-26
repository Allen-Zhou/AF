using TelChina.AF.Sys.Service;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Sys.Configuration;
using System;

namespace TelChina.AF.Demo.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AFConfigurationManager.Setup();
            var agent = ServiceProxy.CreateProxy<ITransSV>();
            var result = agent.Required(true);

            Console.WriteLine(string.Format("更新数据成功!ID为{0}", result));
            Console.ReadLine();
        }
    }
}
