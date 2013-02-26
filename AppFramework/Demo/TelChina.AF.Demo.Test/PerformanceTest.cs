using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;
using TelChina.AF.Util.TestUtil;
using System.Data.SqlClient;
using System.Configuration;
using NHibernate;
using System.Data;

namespace TelChina.AF.Demo.Test
{
    /// <summary>
    /// PerformanceTest 的摘要说明
    /// </summary>
    [TestClass]
    public class PerformanceTest
    {
        public PerformanceTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;
        /// <summary>
        /// 循环次数
        /// </summary>
        private static int ITERATION = 1000;

        private static SqlConnection conn;
        /// <summary>
        ///获取或设置测试上下文，该上下文提供
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
        private static int times = 1;
        private string[] strings =new string[1000] ;
        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            RepositoryContext.Config();
            CodeTimer.Initialize();
            string connString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;
            conn = new SqlConnection(connString);

        }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            if (conn != null)
                conn.Open();

            BatchCreateEntities();
            personList = Person.Finder.FindList();
        }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {
            //删除所有数据
            DeleteAllData();
            conn.Close();
            times = 1;
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        private void DeleteAllData()
        {
            if (conn != null)
            {
                SqlCommand c = new SqlCommand();
                c.CommandText = "Delete from Person";
                c.Connection = conn;
                c.ExecuteNonQuery();
            }
        }
        #endregion
        /// <summary>
        /// 批量新增测试性能测试
        /// </summary>
        [TestMethod]
        public void BatchCreateEntitiesTest()
        {
            CodeTimer.Time("批量新增测试", 1, BatchCreateEntities);
        }
        /// <summary>
        /// 批量单个新增测试性能测试
        /// </summary>
        [TestMethod]
        public void CreateEntityTest()
        {
            CodeTimer.Time("批量单个新增测试", 1000, CreateEntity);
        }
        /// <summary>
        /// 批量插入测试性能测试
        /// </summary>
        [TestMethod]
        public void DataBseInsertTest()
        {
            CodeTimer.Time("批量插入测试", 1000, DataBseInsert);
        }
        #region 新增辅助方法
        /// <summary>
        /// 保存多个Person
        /// </summary>
        private void BatchCreateEntities()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                for (int i = 0; i < ITERATION; i++)
                {
                    var code = "Person-" + (i + 1).ToString();
                    Person person = Person.CreatePerson(code, code, false, Guid.Empty);
                    repo.Add(person);
                    strings[i] = person.ID.ToString();
                }
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 保存一个Person
        /// </summary>
        private static void CreateEntity()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var code = "Person-" + times.ToString();
                Person person = Person.CreatePerson(code, "Person-1120", false, Guid.Empty);
                repo.Add(person);
                repo.SaveChanges();
                times++;
            }
        }

        /// <summary>
        /// 插入一个Person
        /// </summary>
        public static void DataBseInsert()
        {
            var code = "Person-" + times.ToString();
            Person person = Person.CreatePerson(code, "Person-1130", false, Guid.Empty);
            SqlCommand c = Person.InsertBySQLCommand(person, "Insert");
            c.Connection = conn;
            c.ExecuteNonQuery();
            times++;
        }
        #endregion

        public IList<Person> personList;

        /// <summary>
        /// 批量多个修改测试性能测试
        /// </summary>
        [TestMethod]
        public void BatchUpdateEntitiesTest()
        {

            CodeTimer.Time("批量多个修改测试", 1, BatchUpdateEntities);
        }
        /// <summary>
        /// 批量单个修改测试性能测试
        /// </summary>
        [TestMethod]
        public void UpdateEntityTest()
        {
            CodeTimer.Time("批量单个修改测试", 1000, UpdateEntity);
        }
        /// <summary>
        /// 批量更新测试性能测试
        /// </summary>
        [TestMethod]
        public void DataBaseUpdateTest()
        {
            CodeTimer.Time("批量更新测试", 1000, DataBaseUpdate);
        }

        #region 修改辅助方法
        /// <summary>
        /// 修改多个Person
        /// </summary>
        private void BatchUpdateEntities()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                foreach (Person person in personList)
                {
                    person.Name = "修改名称";
                    repo.Update(person);
                }
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 修改一个Person
        /// </summary>
        private void UpdateEntity()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                Person person = personList[times - 1];
                person.Name = "修改一个名称";
                repo.Update(person);
                repo.SaveChanges();
                times++;                
            }
        }

        /// <summary>
        /// 更新一个Person
        /// </summary>
        public void DataBaseUpdate()
        {
            Person person = personList[times - 1];
            person.Name = "修改SQL名称";
            SqlCommand c = Person.InsertBySQLCommand(person, "Update");
            c.Connection = conn;
            c.ExecuteNonQuery();
            times++;
        }
        #endregion

        /// <summary>
        /// 批量多个删除测试性能测试
        /// </summary>
        [TestMethod]
        public void BatchDeleteEntitiesTest()
        {
            CodeTimer.Time("批量多个删除测试", 1, BatchDeleteEntities);
        }
        /// <summary>
        /// 批量单个删除测试性能测试
        /// </summary>
        [TestMethod]
        public void DeleteEntityTest()
        {
            CodeTimer.Time("批量单个删除测试", 1000, DeleteEntity);
        }
        /// <summary>
        /// 批量删除测试性能测试
        /// </summary>
        [TestMethod]
        public void DataBaseDeleteTest()
        {
            CodeTimer.Time("批量删除测试", 1000, DataBaseDelete);

        }

        #region 删除辅助方法
        /// <summary>
        /// 删除多个Person
        /// </summary>
        private void BatchDeleteEntities()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                foreach (Person person in personList)
                {
                    repo.Remove(person);
                }
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 删除一个Person
        /// </summary>
        private void DeleteEntity()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                Person person = personList[times - 1];
                repo.Remove(person);
                repo.SaveChanges();
                times++;
            }
        }

        /// <summary>
        /// 删除一个Person
        /// </summary>
        public void DataBaseDelete()
        {
            Person person = personList[times - 1];
            SqlCommand c = Person.InsertBySQLCommand(person, "Delete");
            c.Connection = conn;
            c.ExecuteNonQuery();
            times++;
        }
        #endregion

        /// <summary>
        /// 通过ID查询性能测试
        /// </summary>
        [TestMethod]
        public void FindByIDTest()
        {
            CodeTimer.Time("通过ID查询", 1, FindByID);
        }

        /// <summary>
        /// 直接执行select语句性能测试
        /// </summary>
        [TestMethod]
        public void SelectByIDTest()
        {
            CodeTimer.Time("直接执行select语句", 1000, SelectByID);
        }
        /// <summary>
        /// NHibernate直接查询性能测试
        /// </summary>
        [TestMethod]
        public void FindByHQLTest()
        {
            CodeTimer.Time("NHibernate直接查询", 1, FindByHQL);
        }

        /// <summary>
        /// 通过条件查询
        /// </summary>
        [TestMethod]
        public void FindByWhereSqlTest()
        {
            CodeTimer.Time("通过Lamda条件查询", 1, FindByWhereSql);
        }

        /// <summary>
        /// 通过条件直接Select性能测试
        /// </summary>
        [TestMethod]
        public void SelectByWhereSqlTest()
        {
            CodeTimer.Time("通过条件直接Select", 1000, SelectByWhereSql);
        }
        #region Find辅助方法
        /// <summary>
        /// 通过ID查询
        /// </summary>
        public void FindByID()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                for (int i = 0; i < ITERATION; i++)
                {
                    repo.GetByID<Person>(Guid.Parse(strings[i]));
                }                    
            }
        }
        /// <summary>
        /// 直接执行select语句
        /// </summary>
        public void SelectByID()
        {
            DataTable inv = new DataTable();
            Guid persinid =Guid.Parse(strings[times-1]);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = @"Select * From[Person] Where ID =@ID";
            sqlCommand.Parameters.Add(new SqlParameter("@ID", persinid));
            sqlCommand.Connection = conn;
            SqlDataReader dr = sqlCommand.ExecuteReader();
            inv.Load(dr);
            times++;
        }
        /// <summary>
        /// 通过条件查询
        /// </summary>
        public void FindByWhereSql()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                foreach (Person person in personList)
                {
                    repo.GetAll<Person>(T => T.Code == person.Code, T => T.Code, false);
                }
            }
        }
        /// <summary>
        /// NHibernate直接查询
        /// </summary>
        public void FindByHQL()
        {
            NHibernateHelper h = new NHibernateHelper();
            ISession session = h.GetCurrentSession();            
            for (int i = 0; i < ITERATION; i++)
            {
                var persons = session.CreateSQLQuery("Select * from Person where ID='" + strings[i] + "'").AddEntity(typeof(Person)).List<Person>();
            }
            
        }
        /// <summary>
        /// 通过条件直接Select
        /// </summary>
        public void SelectByWhereSql()
        {
            DataTable inv = new DataTable();

            Person person = personList[times - 1];
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = @"Select * From[Person] Where Code =@Code Order by Code";
            sqlCommand.Parameters.Add(new SqlParameter("@Code", person.Code));
            sqlCommand.Connection = conn;
            SqlDataReader dr = sqlCommand.ExecuteReader();
            inv.Load(dr);
            // sqlCommand.ExecuteNonQuery();
            times++;
        }
        #endregion
    }
}
