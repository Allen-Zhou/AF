using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.TRF.Demo;
using TelChina.TRF.Persistant;

namespace TelChina.TRF.Demo.Test
{
    /// <summary>
    /// EntityBaseStateTest 的摘要说明
    /// </summary>
    [TestClass]
    public class EntityBaseStateTest
    {
        public EntityBaseStateTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        private Guid id = Guid.Parse("893F91E1-49FA-4819-8C4F-B4994F6B0665");

        private TestContext testContextInstance;

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


        public void SavePerson()
        {
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            Person person = new Person();
            person.ID = Guid.NewGuid();
            person.Code = "Person_001";
            person.Name = "Name_001";
            person.CreatedOn = DateTime.Now;
            person.CreatedBy = "DS";
            person.UpdatedBy = "DS-1";
            person.UpdatedOn = DateTime.Now;
            repository.Add(person);
            repository.SaveChanges();
        }
        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region 新增测试
        /// <summary>
        /// 测试New，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void CreateNewTest()
        {
            /*
                  实体=New  
                 * 断言实体状态 PL中存在
                 */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            Person person = new Person();            
            Assert.AreEqual(person.SysState, EntityStateEnum.Inserting);
        }

        /// <summary>
        /// 测试New，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void CreateNewSaveTest()
        {
            /*
                  实体=New  
                 * 断言实体状态 PL中存在
                 */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            Person person = new Person();
            person.ID = Guid.NewGuid();
            person.Code = "Person_001";
            person.Name = "Name_001";
            person.CreatedOn = DateTime.Now;
            person.CreatedBy = "DS";
            person.UpdatedBy = "DS-1";
            person.UpdatedOn = DateTime.Now;
            repository.Add(person);
            repository.SaveChanges();
            Assert.AreEqual(person.SysState, EntityStateEnum.Unchanged);
        }
        #endregion

        #region 查找测试
        /// <summary>
        /// 测试查找，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void FindTest()
        {
            /*
                  实体=Finder 
                 * 断言实体状态 PL中存在
                 */            
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            Assert.AreEqual(person.SysState, EntityStateEnum.Unchanged);
            repository.SaveChanges();
        }
        #endregion
        /// <summary>
        /// 测试finder之后修改属性为本身，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void UpdateFindSelfTest()
        {
            /*
                实体=PL.Finder
             * 实体.属性=本身
             * 断言
             */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            person.Code = person.Code;
            Assert.AreEqual(person.SysState, EntityStateEnum.Unchanged);
            repository.SaveChanges();
        }

        #region 修改测试
        /// <summary>
        /// 测试finder之后修改属性，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void UpdateFindTest()
        {
            /*
                实体=PL.Finder
             * 实体.属性=改变
             * 断言
             */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            person.Code = "567";
            Assert.AreEqual(person.SysState, EntityStateEnum.Updating);
            repository.SaveChanges();
        }        

        /// <summary>
        /// 测试新增实体之后修改属性，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void UpdateNewTest()
        {
            /*
                实体=NEW
             * 实体.属性=改变
             * 断言
             */
            Person person = new Person();
            person.Code = "567";
            Assert.AreEqual(person.SysState, EntityStateEnum.Inserting);
        }

        #endregion

        #region 删除测试
        /// <summary>
        /// 测试finder之后实体删除，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void RemoveFindingTest()
        {
            /*
                实体=PL.Finder
             * 实体.Delete
             * 断言
             */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            person.Remove();
            Assert.AreEqual(person.SysState, EntityStateEnum.Deleting);
            repository.SaveChanges();
        }

        /// <summary>
        /// 测试finder之后修改实体属性，实体删除，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void RemoveupdateTest()
        {
            /*
                实体=PL.Finder
             * 实体.属性=改变
             * 实体.Delete
             * 断言
             */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            person.Code = "567";
            person.Remove();
            Assert.AreEqual(person.SysState, EntityStateEnum.Deleting);
            repository.SaveChanges();
        }

        /// <summary>
        /// 测试新增实体，实体删除，
        /// </summary>
        [TestMethod()]
        public void RemoveNewTest()
        {
            /*
                实体=NEW
             * 实体.Delete
             */
            Person person = new Person();
            person.Code = "567";
            person.Remove();
            Assert.AreEqual(person.SysState, EntityStateEnum.Detached);
        }
        #endregion

        #region Detach测试
        /// <summary>
        /// 测试新增实体，Detached 实体状态以及PL中是否存在
        /// </summary>
        [TestMethod()]
        public void DetachedNewTest()
        {
            /*
                实体=NEW
             * 实体.Detached
             * 断言  实体状态以及PL中是否存在
             */
            Person person = new Person();
            person.Code = "567";
            person.Detached();
            Assert.AreEqual(person.SysState, EntityStateEnum.Detached);
        }

        /// <summary>
        /// 测试Unchanged状态实体，Detached 实体状态以及PL中是否存在
        /// </summary>
        [TestMethod()]
        public void DetachedFindTest()
        {
            /*
               实体=Finder
             * 实体.Detached
             * 断言  实体状态以及PL中是否存在
             */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            person.Detached();
            Assert.AreEqual(person.SysState, EntityStateEnum.Detached);
            repository.SaveChanges();
        }

        /// <summary>
        /// 测试修改状态实体，Detached 实体状态以及PL中是否存在
        /// </summary>
        [TestMethod()]
        public void DetachedUpdateTest()
        {
            /*
                实体=Finder
              * 实体.属性=改变
              * 实体.Detached
              * 断言  实体状态以及PL中是否存在
              */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            person.Code = "567";
            person.Detached();
            Assert.AreEqual(person.SysState, EntityStateEnum.Detached);
            repository.SaveChanges();
        }

        /// <summary>
        /// 测试删除状态的实体，Detached 实体状态以及PL中是否存在
        /// </summary>
        [TestMethod()]
        public void DetachedDeletingTest()
        {
            /*
               实体=Find
             * 实体.Deleting
             * 实体.Detached
             * 断言  实体状态以及PL中是否存在
             */
            IRepository<Person> repository = RepositoryContext.CreateRepository<Person>();
            IList<Person> personList = repository.GetAll();
            if (personList.Count == 0)
            {
                SavePerson();
                personList = repository.GetAll();
            }
            Person person = personList[0];
            person.Remove();
            person.Detached();
            Assert.AreEqual(person.SysState, EntityStateEnum.Detached);
            repository.SaveChanges();
        }
        #endregion
    }
}
