using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Demo;
using TelChina.AF.Persistant;
using TelChina.AF.Util.Logging;
using TelChina.AF.Util.TestUtil;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Util.Common;
using TelChina.AF.Service.AppHosting;
using TelChina.AF.Sys.Service;
using System.Runtime.Remoting.Messaging;
using TelChina.AF.Test.DemoSV.Test.TransSVDirect;
using ITransSV = TelChina.AF.Demo.DemoSV.ITransSV;
using System.Threading.Tasks;
using TelChina.AF.Sys.Context;

namespace TelChina.AF.Test.DemoSV.Test
{
    [TestClass]
    public class PerfomanceTestForSV
    {
        private IRepository _repository;
        private static ILogger logger = LogManager.GetLogger("SVTransUnitTest");
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //没有这句,impl.Dll不会自动复制到测试目录下,导致无法正确执行服务加载
            CodeTimer.Initialize();
            var sv = new TransSV();
            sv = null;
            logger.Debug("服务正在启动...");
            if ("Integrate" == ConfigHelper.GetConfigValue("DeployType"))
            {
                AppHost.Start();
                logger.Debug("服务已经启动");
            }
            RepositoryContext.Config();
        }

        [ClassCleanup]
        public static void MyClassCleanup()
        {
            logger.Debug("服务正在关闭...");
            if ("Integrate" == ConfigHelper.GetConfigValue("DeployType"))
            {
                AppHost.Stop();
                logger.Debug("服务已经结束");
            }
        }
        private static ServiceContext GetProfileContext()
        {
            return new ServiceContext
            {
                LoginDate = DateTime.Now,
                UserCode = "admin",
                UserName = "admin",
                LoginIP = "127.0.0.1",
                UserID = "123",
                Token = Guid.NewGuid().ToString(),
                Content = new Dictionary<string, string>() { { "ContentKey1", "ContentValue1" }, { "ContentKey2", "ContentValue2" } }

            };
        }
        [TestInitialize]
        public void MyTestInitialize()
        {


            var context = GetProfileContext();
            ContextSession.Current = context;
            //CallContext.SetData(ServiceContextManager<ServiceContext>.SESSIONCOTEXTKEY, context);

            _repository = RepositoryContext.GetRepository();
            //清理垃圾数据
            var rubbish = _repository.GetAll<Category>();
            if (rubbish == null || rubbish.Count() <= 0) return;
            foreach (var item in rubbish)
                this._repository.Remove(item);

            _repository.SaveChanges();
        }
        [TestCleanup]
        public void MyTestClearup()
        {

        }
        [TestMethod]
        public void PreformanceTest()
        {
            CodeTimer.Time("Proxy方式服务批量调用", 1000, () =>
            {
                var proxy = ServiceProxy.CreateProxy<ITransSV>();
                proxy.Supported();
            });

            CodeTimer.Time("Agent方式服务批量调用", 1000, () =>
            {
                var sv = new TransSVAgent();
                sv.Supported(true);
                sv.Close();
            });
            CodeTimer.Time("IIS 承载方式服务批量调用", 1000, () =>
            {
                var client = new TransSVClient();
                client.Supported();
                client.Close();
            }
            );
        }
        [TestMethod]
        public void ParallelTest()
        {
            CodeTimer.Time("IIS 承载方式服务批量调用", 1000, () =>
                                                       {
                                                           var client = new TransSVClient();
                                                           client.Supported();
                                                           client.Close();
                                                       });
        }
        [TestMethod]
        public void Test()
        {
            CodeTimer.Time("Web方式服务批量调用", 1, () => Parallel.For(0, 1000, x =>
            {
                var client = new IISService.MyServiceClient();
                client.DoWork();
                client.Close();
            }
            ));

        }


        [TestMethod]
        public void LoadTest()
        {
            var sv = new TransSVAgent();
            sv.Required(true);
        }

        [TestMethod]
        public void FirstCallPerformanceTest()
        {
            var transSv = ServiceProxy.CreateProxy<ITransSV>();
            transSv.Required(true);

            var demoSV = ServiceProxy.CreateProxy<IDemoSV>();
            demoSV.Do(new ParamDTO() { ParamName = "PerfomanceTest", IsSucceed = true, Value = "Test" });
        }
    }
}
