using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;
using TelChina.AF.Util.TestUtil;

namespace TelChina.AF.Demo.Test
{
    [TestClass]
    public class DynamicSqlTest
    {
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            RepositoryContext.Config();
            CodeTimer.Initialize();
        }


        [TestInitialize()]
        public void MyTestInitialize()
        {
            // RepositoryContext.Config();
            //BatchCreateEntities();
            //CodeTimer.Initialize();
        }

        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {
            //删除所有数据
            //DeleteAllData();

            times = 1;
        }


        private TestContext testContextInstance;

        /// <summary>
        /// 循环次数
        /// </summary>
        private static int ITERATION = 100;


        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private static int times = 1;
        private string[] strings = new string[1000];

        /// <summary>
        /// 删除所有数据
        /// </summary>
        private void DeleteAllData()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var result = repo.GetAll<Category>();
                foreach (var item in result)
                {
                    repo.Remove(item);
                }
                repo.SaveChanges();
            }
        }


        /// <summary>
        /// 保存多个Person
        /// </summary>
        private void BatchCreateEntities()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                for (int i = 0; i < ITERATION; i++)
                {
                    var name = "Category-" + (i + 1).ToString();
                    Category cate = new Category() {Name = name, Size = i};
                    repo.Add(cate);
                    strings[i] = cate.ID.ToString();
                }
                repo.SaveChanges();
            }
        }

        private void SelectALLTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {

                repo.ExecuteQuery<Category, DynamicSqlResponse>("GetAllCategory", null);
            }
        }

        #region 性能测试

        /// <summary>
        /// 直接执行select语句性能测试
        /// </summary>
        [TestMethod]
        public void SelectAllTest()
        {
            CodeTimer.Time("通过动态sql查询返回实体集合", 100, SelectALLTest);
        }

        #endregion


        #region  基本查询Test


        [TestMethod]
        public void TestNormalStatement()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var request = new DynamicSqlRequest() {Size = 0, Name = "Add"};

                var result = repo.ExecuteQuery<Category, DynamicSqlResponse>("GetCategory", request);
                Console.WriteLine(result[0].Name);
                Assert.IsTrue(result != null && result.Count > 0);
            }
        }

        [TestMethod]
        public void TestStatement_In()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var request = new DynamicSqlRequest() {Size = 0, SizeArray = new List<int> {1, 2}};

                var result = repo.ExecuteQuery<Category, DynamicSqlResponse>("GetCategoryBySize", request);
                Assert.IsTrue(result.GetType() == typeof (List<DynamicSqlResponse>));
                Assert.IsTrue(result != null && result.Count > 0);
            }
        }

        [TestMethod]
        public void TestStatement_Map()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var request = new Hashtable();
                var a = new int[] {0, 1};
                request.Add("SizeList", a);

                var result = repo.ExecuteQuery<Category>("GetCategoryByMap", request);
                Assert.IsTrue(result[0].ContainsKey("Size"));
                Assert.IsTrue(result[0].ContainsKey("Name"));
                Assert.IsTrue(result != null && result.Count > 0);
            }
        }
        [TestMethod]
        public void TestPro_Map()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var request = new Hashtable();
                request.Add("firstParam", "Category-21");
                request.Add("secondParam", 20);

                var result = repo.ExecuteQuery<Category,Category>("getAllCategoryByPro", request);
            }
        }

        /// <summary>
        /// Ibatis存储过程
        /// </summary>
        [TestMethod]
        public void TestProcedure_Delete()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var id = new DeleteProcedureClass();
                var addedcategory = repo.GetAll<Category>();
                id.ID = addedcategory[0].ID;
                var builder = new DomSqlMapBuilder();
                var configfileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "SqlMap.config");
                ISqlMapper mapper = builder.Configure(configfileName);                
                mapper.Delete("DeleteProcedureTest", id);
            }
        }

        /// <summary>
        /// 测试删除的存储过程
        /// 在hbm中已经修改
        /// </summary>
        [TestMethod]
        public void DeleteProcedure()
        {
           using (var repo = RepositoryContext.GetRepository())
           {
                var addedcategory = repo.GetAll<Category>();
                var test = addedcategory[0];
                repo.Remove(test);
                repo.SaveChanges();
            }
        }
        #endregion
    }
}
