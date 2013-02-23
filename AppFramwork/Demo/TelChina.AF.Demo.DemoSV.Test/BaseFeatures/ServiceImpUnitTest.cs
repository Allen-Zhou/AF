using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Sys.Service;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using TelChina.AF.Service.AppHosting;
using TelChina.AF.Util.Common;
using TelChina.AF.Sys.Context;
//using System.ServiceModel.Channels;

namespace TelChina.AF.Test.ServiceTest
{
    [TestClass]
    public class ServiceImpUnitTest
    {
        [TestInitialize]
        public void MyTestInitialize()
        {
            if ("Integrate" == ConfigHelper.GetConfigValue("DeployType"))
                AppHost.Start();
        }


        private static ParamDTO GetParam()
        {
            return new ParamDTO
                       {
                           IsSucceed = true,
                           ParamName = "ParamName=P1",
                           Value = "ParamValue =V1"
                       };
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

        [TestMethod]
        public void ServiceAgentInvokeTest()
        {
            var context = GetProfileContext();
            ContextSession.Current = context;
            //CallContext.SetData(ServiceContextManager<ServiceContext>.SESSIONCOTEXTKEY, context);

            var agent = new DemoSVAgent();
            agent.Param = GetParam();
            agent.Do();
        }
    }
}
