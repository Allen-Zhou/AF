using TelChina.AF.Demo.DemoSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TelChina.AF.Sys.Configuration;
using TelChina.AF.Service.AppHosting;
using System.Threading;

namespace TelChina.AF.Test.DemoSV.Test
{


    /// <summary>
    ///This is a test class for DualSVAgentTest and is intended
    ///to contain all DualSVAgentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DualSVAgentTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            AFConfigurationManager.Setup();
            AppHost.Start();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Do
        ///</summary>
        [TestMethod()]
        public void DoTest()
        {
            DualSVAgent target = new DualSVAgent();
            target.Do();
        }

        [TestMethod()]
        public void ClientTest()
        {
            var target = new DualSVAgent<IDualSVCallback>();
            target.Connect();
            target.Excute("ServiceInvoke", "Need Callback!");


            //var callBack = new BizDualSVImpl();
            //callBack.DoCallBack("Test CallBack!");

            target.DisConnect();
        }
    }
}
