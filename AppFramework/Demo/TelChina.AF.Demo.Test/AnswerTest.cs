using TelChina.AF.Demo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TelChina.AF.Sys.Configuration;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.Test
{


    /// <summary>
    ///This is a test class for AnswerTest and is intended
    ///to contain all AnswerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AnswerTest
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
            RepositoryContext.Config();
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
        ///A test for SetDefaultValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TelChina.AF.Demo.dll")]
        public void PreEventTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {

                var q = new Question() { Name = "Who are you?" };
                var a1 = new Answer() { Name = "I'm ZhangSan！", Question_ID = q.ID };
                var a2 = new Answer() { Name = "I'm LiSi！", Question_ID = q.ID };

                repo.Add(q);
                repo.Add(a1);
                repo.Add(a2);
                repo.SaveChanges();

                repo.Remove(q);
                repo.SaveChanges();
                var result = repo.GetAll<Answer>(a => a.Question_ID == q.ID);
                Assert.IsTrue(result != null && result.Count == 0);
            }
        }
    }
}
