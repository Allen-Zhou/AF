using System.Linq;
using TelChina.AF.Demo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TelChina.AF.Persistant;
using System.Collections.Generic;

namespace TelChina.AF.Demo.Test
{


    /// <summary>
    ///这是 DepartmentTest 的测试类，旨在
    ///包含所有 DepartmentTest 单元测试
    ///</summary>
    [TestClass()]
    public class DepartmentTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region 附加测试特性

        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            RepositoryContext.Config();

            ClearData();
        }

        private void ClearData()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var persons = repo.GetAll<Person>();
                foreach (var person in persons)
                {
                    repo.Remove(person);
                }

                var departments = repo.GetAll<Department>();

                foreach (var department in departments)
                {
                    repo.Remove(department);
                }

                repo.SaveChanges();
            }
        }

        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {
            //ClearData();
        }

        //

        #endregion


        /// <summary>
        /// 实体新增
        /// </summary>
        [TestMethod()]
        public void Add_NormalTest()
        {
            var repo = RepositoryContext.GetRepository();
            var department = new Department();
            repo.Add(department);
            Assert.AreEqual(department.SysState, EntityStateEnum.Inserting);

            repo.SaveChanges();
            Assert.AreEqual(department.SysState, EntityStateEnum.Unchanged);

            var addedDepartment = RepositoryContext.GetRepository().GetByID<Department>(department.ID);
            Assert.IsNotNull(addedDepartment);
            Assert.AreEqual(addedDepartment.SysState, EntityStateEnum.Unchanged);
        }


        /// <summary>
        /// 测试二进制数
        /// </summary>
        [TestMethod()]
        public void LogClassAddTest()
        {
            LogClass logClass = new LogClass();
            logClass.SaveLog("Add");
        }

        [TestMethod()]
        public void LogClassGetTest()
        {
            IList<LogClass> logClassLists;
            using (var repo = RepositoryContext.GetRepository())
            {
                logClassLists = repo.GetAll<LogClass>();
            }
        }

        /// <summary>
        /// 实体one-to-many测试
        /// </summary>
        [TestMethod]
        public void EntityOTMTest()
        {
            var DepartmentTest = new Department();
            DepartmentTest.AddChild(Person.CreatePerson("Person-1005", "张三", false,
                                                        Guid.Parse(
                                                            "B2DD9318-7A40-4D44-A5C6-26DCA237E091")));
            DepartmentTest.AddChild(Person.CreatePerson("Person-1007", "王五", true,
                                                        Guid.Parse(
                                                            "995A7640-3303-47DF-AEC2-20E459CDFEC3")));
            DepartmentTest.AddChild(Person.CreatePerson("Person-1006", "李四", false,
                                                        Guid.Parse(
                                                            "91FBED4E-A754-4022-AE3D-7FF9C21861CC")));

            using (var repo = RepositoryContext.GetRepository())
            {
                repo.Add(DepartmentTest);
                Assert.AreEqual(DepartmentTest.SysState, EntityStateEnum.Inserting);

                repo.SaveChanges();
                Assert.AreEqual(DepartmentTest.SysState, EntityStateEnum.Unchanged);

                var addedcategory = RepositoryContext.GetRepository().GetByID<Category>(DepartmentTest.ID);
                Assert.IsNotNull(addedcategory);
                Assert.AreEqual(addedcategory.SysState, EntityStateEnum.Unchanged);
            }
        }

        /// <summary>
        /// 实体one-to-many测试
        /// </summary>
        [TestMethod]
        public void EntityChildInsertTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addChild = Person.CreatePerson("Person-1016", "张三", false,
                                                   Guid.Parse(
                                                       "B2DD9318-7A40-4D44-A5C6-26DCA237E091"));
                var department = new Department() {Name = "NewTest1122"};
                addChild.Department = department;
                repo.Add(addChild);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// ID做持久化验证
        /// </summary>
        [TestMethod]
        public void EntityIDAndDeptTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addChild = Person.CreatePerson("P1122", "Test1122", false,
                                                   Guid.Parse(
                                                       "B2DD9318-7A40-4D44-A5C6-26DCA567E091"));
                var department = new Department() {Name = "NewTest1122"};
                addChild.idDepartment = department.ID;
                repo.Add(addChild);
                repo.Add(department);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 实体ID持久化之后Set测试以及Linq lamda表达式测试
        /// </summary>
        [TestMethod]
        public void EntityIDSetTest()
        {
            Guid deptID;
            using (var repo = RepositoryContext.GetRepository())
            {
                var addChildA = Person.CreatePerson("P1122A", "Test1122A", false,
                                                    Guid.Parse(
                                                        "B2DD9318-7A40-4D44-A5C6-26DCA567E091"));
                var addChildB = Person.CreatePerson("P1122B", "Test1122B", false,
                                                    Guid.Parse(
                                                        "B2DD9318-7A40-4D44-A5C6-26DCA567E092"));
                var department = new Department() {Name = "NewTest1122"};
                addChildA.idDepartment = department.ID;
                addChildB.idDepartment = department.ID;
                repo.Add(addChildA);
                repo.Add(addChildB);
                repo.Add(department);
                repo.SaveChanges();

                deptID = department.ID;
                var testDepartment = repo.GetByID<Department>(department.ID);
                Assert.AreEqual(testDepartment.Persons.Count, 0);

            }
            using (var repo = RepositoryContext.GetRepository())
            {
                var testDepartment = repo.GetByID<Department>(deptID);

                /// 能否使用导航属性作为兰姆达表达式中的参数                
                var persons =
                    repo.GetAll<Person>(p => p.idDepartment == testDepartment.Persons.FirstOrDefault().Department.ID);

                foreach (var person in persons)
                {
                    Assert.IsNotNull(person.Department, "一对多的多端，关系断裂");
                }
                Assert.AreEqual(testDepartment.Persons.Count, 2, "一对多的一端，关系断裂");
            }
            // 导航属性作为排序依据
            using (var repo = RepositoryContext.GetRepository())
            {
                var testDepartment = repo.GetByID<Department>(deptID);
                var result = (from item in repo.Query<Person>()
                              where item.idDepartment == testDepartment.Persons.FirstOrDefault().Department.ID
                              select item).ToList<Person>();
            }           
        }

        /// <summary>
        /// 用ID做持久化的级联save测试
        /// 结果：不可以级联save测试
        /// </summary>
        [TestMethod]
        public void EntitySetSaveTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addChildA = Person.CreatePerson("P1122A", "Test1122A", false,
                                                    Guid.Parse(
                                                        "B2DD9318-7A40-4D44-A5C6-26DCA567E091"));
                var addChildB = Person.CreatePerson("P1122B", "Test1122B", false,
                                                    Guid.Parse(
                                                        "B2DD9318-7A40-4D44-A5C6-26DCA567E092"));
                var department = new Department() {Name = "NewTest1122"};
                addChildA.idDepartment = department.ID;
                addChildB.idDepartment = department.ID;
                repo.Add(department);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 修改过配置文件
        /// 实体做持久化懒加载测试
        /// </summary>
        [TestMethod]
        public void EntitySetLazyTest()
        {
            Guid deptID;
            // 验证用实体做持久化可以级联保存
            using (var repo = RepositoryContext.GetRepository())
            {
                var addChildA = Person.CreatePerson("P1122A", "Test1122A", false,
                                                    Guid.Parse(
                                                        "B2DD9318-7A40-4D44-A5C6-26DCA567E091"));
                var addChildB = Person.CreatePerson("P1122B", "Test1122B", false,
                                                    Guid.Parse(
                                                        "B2DD9318-7A40-4D44-A5C6-26DCA567E092"));
                var department = new Department() {Name = "NewTest1122"};

                department.Persons.Add(addChildA);
                department.Persons.Add(addChildB);

                deptID = department.ID;

                repo.Add(department);

                repo.SaveChanges();
            }

            using (var repo = RepositoryContext.GetRepository())
            {
                var testDepartment = repo.GetByID<Department>(deptID);

                foreach (var person in testDepartment.Persons)
                {

                }
            }
        }
        
    }

}
