using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;
using TelChina.AF.Demo;

namespace TelChina.AF.Demo.Test
{
    [TestClass]
    public class SystemResourceTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            RepositoryContext.Config();
        }
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///save 的测试
        ///</summary>
        [TestMethod()]
        public void saveTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                SystemResource sysResource = new SystemResource();
                sysResource.ClassName = "User";
                sysResource.ColumnDescribe = "用户";
                sysResource.IsVisible = true;
                repo.Add(sysResource);

                SystemResource sysResource1 = new SystemResource();
                sysResource1.ClassName = "User";
                sysResource1.ColumnName = "Code";
                sysResource1.ColumnDescribe = "编码";
                sysResource1.IsVisible = true;
                repo.Add(sysResource1);


                SystemResource sysResource2 = new SystemResource();
                sysResource2.ClassName = "User";
                sysResource2.ColumnName = "Name";
                sysResource2.ColumnDescribe = "名称";
                sysResource2.IsVisible = true;
                repo.Add(sysResource2);

                SystemResource sysResource3 = new SystemResource();
                sysResource3.ClassName = "User";
                sysResource3.ColumnName = "Adress";
                sysResource3.ColumnDescribe = "地址";
                sysResource3.IsVisible = true;
                repo.Add(sysResource3);
                repo.SaveChanges();
            }
        }

        /// <summary>
        ///save 的测试
        ///</summary>
        [TestMethod()]
        public void saveUserResourceTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                Useresource userResource = new Useresource();
                userResource.ClassName = "User";
                userResource.ColumnName = "Telphone";
                userResource.ColumnDescribe = "用户电话";
                userResource.IsVisible = true;
                repo.Add(userResource);
                repo.SaveChanges();
            }
        }
    }
}
