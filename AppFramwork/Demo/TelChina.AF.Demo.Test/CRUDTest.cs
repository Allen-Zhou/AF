using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.DemoB;
using TelChina.AF.Persistant;
using TelChina.AF.Persistant.Exceptions;
using System.Runtime.Remoting.Messaging;
using TelChina.AF.Persistant.NHImpl;
using TelChina.AF.Sys.Exceptions;
using NHibernate;

namespace TelChina.AF.Demo.Test
{
    /// <summary>
    /// Summary description for CRUDTest
    /// </summary>
    [TestClass]
    public class CRUDTest
    {
        public CRUDTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        [TestInitialize()]
        public void MyTestInitialize()
        {
            RepositoryContext.Config();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion



        #region  单实体 CUD


        #region  新增
        /// <summary>
        /// 测试finder之后修改属性，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void Add_NormalTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "Add_NormalTest" };
                repo.Add(category);
                Assert.AreEqual(category.SysState, EntityStateEnum.Inserting);

                repo.SaveChanges();
                Assert.AreEqual(category.SysState, EntityStateEnum.Unchanged);

                var addedcategory = RepositoryContext.GetRepository().GetByID<Category>(category.ID);
                Assert.IsNotNull(addedcategory);
                Assert.AreEqual(addedcategory.SysState, EntityStateEnum.Unchanged);
            }
        }

        /// <summary>
        /// 测试从数据库查处一条，赋值给一个新实体，然后新增该实体
        /// 期望结果： 新增操作有效
        /// </summary>
        [TestMethod()]
        public void AddFromFind_SuccessTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "AddFromFind_Test" };
                repo.Add(category);
                repo.SaveChanges();

                var addedCategory = repo.GetByID<Category>(category.ID);
                var readdedCategory = new Category() { Name = category.Name };
                repo.Add(readdedCategory);
                repo.SaveChanges();
                var firstAddCategory = repo.GetByID<Category>(category.ID);
                var secondAddCategory = repo.GetByID<Category>(readdedCategory.ID);
                Assert.IsTrue(firstAddCategory != secondAddCategory);
                Assert.IsTrue(firstAddCategory.ID != secondAddCategory.ID);
                Assert.IsNotNull(firstAddCategory);
                Assert.IsNotNull(secondAddCategory);
            }
        }


        /// <summary>
        /// 测试从数据库查处一条，删除该条，提交， 然后复制一份该条记录，新增、提交
        /// 期望结果： 新增操作失败
        /// </summary>
        [TestMethod()]
        public void AddFromDelete_SuccessTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "AddFromDelete_FailTest" };
                repo.Add(category);
                repo.SaveChanges();

                var addedCategory = repo.GetByID<Category>(category.ID);
                repo.Remove(addedCategory);
                repo.SaveChanges();
                var readdedCategory = new Category() { Name = category.Name };
                repo.Add(readdedCategory);
                repo.SaveChanges();
                var firstAddCategory = repo.GetByID<Category>(category.ID);
                var secondAddCategory = repo.GetByID<Category>(readdedCategory.ID);
                Assert.IsNull(firstAddCategory);
                Assert.IsNotNull(secondAddCategory);
            }
        }
        #endregion

        #region 修改测试
        /// <summary>
        /// 测试finder之后修改属性，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void Update_NormalTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "Update_NormalTest" };
                repo.Add(category);
                Assert.AreEqual(category.SysState, EntityStateEnum.Inserting);

                repo.SaveChanges();
                Assert.AreEqual(category.SysState, EntityStateEnum.Unchanged);

                var addedcategory = repo.GetByID<Category>(category.ID);
                Assert.AreEqual(addedcategory.SysState, EntityStateEnum.Unchanged);

                addedcategory.Name = "Test1345";
                repo.Update(addedcategory);

                Assert.AreEqual(addedcategory.SysState, EntityStateEnum.Updating);
                repo.SaveChanges();
            }
        }

        #endregion

        #region 删除测试
        /// <summary>
        ///  新增一个实体，提交，然后查处该实体，删除该实体,提交
        /// 期望结果：新增成功，删除成功
        /// </summary>
        [TestMethod()]
        public void Remove_NormalTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { };
                repo.Add(category);
                repo.SaveChanges();

                var addedCategory = repo.GetByID<Category>(category.ID);
                repo.Remove(addedCategory);
                repo.SaveChanges();

                var deletedCategory = repo.GetByID<Category>(category.ID);
                Assert.IsNull(deletedCategory);
            }
        }

        /// <summary>
        ///  新增一个实体,然后删除该实体，提交
        /// 期望结果： 实体最终在数据库不存在
        /// </summary>
        [TestMethod()]
        public void RemoveFromAdded_SuccessTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "RemoveFromAdded_SuccessTest" };
                repo.Add(category);
                repo.Remove(category);
                repo.SaveChanges();
                var addedCategory = repo.GetByID<Category>(category.ID);
                Assert.IsNull(addedCategory);
            }
        }

        /// <summary>
        ///  新增一个实体,然后更新，删除，提交
        /// 期望结果： 实体最终从数据库中删除
        /// </summary>
        [TestMethod()]
        public void RemoveFromUpdated_SuccessTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "RemoveFromUpdated_SuccessTest" };
                repo.Add(category);
                repo.SaveChanges();
                var addedCategory = repo.GetByID<Category>(category.ID);
                addedCategory.Name = "RemoveFromUpdated_SuccessTest_Updated";
                repo.Update(addedCategory);
                repo.Remove(category);
                repo.SaveChanges();
                var deletedCategory = repo.GetByID<Category>(category.ID);
                Assert.IsNull(deletedCategory);
            }
        }
        /// <summary>
        /// 模拟前台传回ID和SysVersion时删除的动作
        /// </summary>
        [TestMethod]
        public void RemoveByDTO()
        {
            AnswerDTO answerDTO;
            using (var repo = RepositoryContext.GetRepository())
            {
                var answer = new Answer();
                answer.Name = "RemoveByDTO";
                repo.Add(answer);
                repo.SaveChanges();
                answer = repo.GetByID<Answer>(answer.ID);
                Assert.IsNotNull(answer, "删除测试中,准备数据插入失败");

                //创建实体给前台使用，其他字段就不一一列举了，关键的是ID，和SysVersion字段
                answerDTO = new AnswerDTO() { ID = answer.ID, SysVersion = answer.SysVersion };
            }

            using (var repo = RepositoryContext.GetRepository())
            {
                var entityToDelete = new Answer()
                {
                    ID = answerDTO.ID,
                    SysVersion = answerDTO.SysVersion
                };
                repo.Remove(entityToDelete);
                repo.SaveChanges();

                var answerResult = repo.GetByID<Answer>(answerDTO.ID);
                Assert.IsNull(answerResult, "通过ID和SysVersion没有删除成功");
            }

        }

        public class AnswerDTO
        {
            public Guid ID { get; set; }
            public int SysVersion { get; set; }
        }
        #endregion

        #endregion

        #region 多实体 CUD

        #region  新增
        /// <summary>
        /// 测试finder之后修改属性，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void Multi_Add_NormalTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category1 = new Category() { Name = "Multi_Add_NormalTest1" };
                repo.Add(category1);
                var category2 = new Category() { Name = "Multi_Add_NormalTest2" };
                repo.Add(category2);
                repo.SaveChanges();
                var addedcategory1 = repo.GetByID<Category>(category1.ID);
                var addedcategory2 = repo.GetByID<Category>(category2.ID);
                Assert.IsNotNull(addedcategory1);
                Assert.AreEqual(addedcategory1.Name, "Multi_Add_NormalTest1");

                Assert.IsNotNull(addedcategory2);
                Assert.AreEqual(addedcategory2.Name, "Multi_Add_NormalTest2");
            }
        }
        #endregion

        #region 修改测试
        /// <summary>
        /// 测试finder之后修改属性，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void Multi_Update_NormalTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category1 = new Category() { Name = "Multi_Update_NormalTest1" };
                var category2 = new Category() { Name = "Multi_Update_NormalTest2" };
                repo.Add(category1);
                repo.Add(category2);
                repo.SaveChanges();

                var addedcategory1 = repo.GetByID<Category>(category1.ID);
                var addedcategory2 = repo.GetByID<Category>(category2.ID);
                Assert.IsNotNull(addedcategory1);
                Assert.IsNotNull(addedcategory2);

                addedcategory1.Name = addedcategory1.Name + "_update";
                addedcategory2.Name = addedcategory2.Name + "_update";


                repo.Update(addedcategory1);
                repo.Update(addedcategory2);
                repo.SaveChanges();

                var updatecategory1 = repo.GetByID<Category>(addedcategory1.ID);
                var updatecategory2 = repo.GetByID<Category>(addedcategory2.ID);

                Assert.AreEqual(updatecategory1.Name, "Multi_Update_NormalTest1_update");
                Assert.AreEqual(updatecategory2.Name, "Multi_Update_NormalTest2_update");
            }
        }

        #endregion

        #region 删除测试
        /// <summary>
        ///  新增两个个实体，提交，然后查处该实体，删除该实体,提交
        /// 期望结果：新增成功，删除成功
        /// </summary>
        [TestMethod()]
        public void MultiRemove_NormalTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category1 = new Category() { Name = "MultiRemove_NormalTest" };
                var category2 = new Category() { Name = "MultiRemove_NormalTest" };
                repo.Add(category1);
                repo.Add(category2);
                repo.SaveChanges();

                var addedcategory1 = repo.GetByID<Category>(category1.ID);
                var addedcategory2 = repo.GetByID<Category>(category2.ID);
                Assert.IsNotNull(addedcategory1);
                Assert.IsNotNull(addedcategory2);

                repo.Remove(addedcategory1);
                repo.Remove(addedcategory2);
                repo.SaveChanges();

                var deletedcategory1 = repo.GetByID<Category>(addedcategory1.ID);
                var deletedcategory2 = repo.GetByID<Category>(addedcategory2.ID);
                Assert.IsNull(deletedcategory1);
                Assert.IsNull(deletedcategory2);
            }

        }


        #endregion

        #endregion

        #region DTO 转换成Entity 执行更新操作

        #region 修改测试
        /// <summary>
        ///  DTO 转换成Entity 执行更新操作
        /// </summary>
        [TestMethod()]
        public void DTO_Update_NormalTest()
        {
            var category = new Category() { Name = "DTO_Update_NormalTest" };
            using (var repo = RepositoryContext.GetRepository())
            {
                repo.Add(category);
                Assert.AreEqual(category.SysState, EntityStateEnum.Inserting);
                repo.SaveChanges();
            }

            var DTO_T0_Entity_category = new Category()
            {
                ID = category.ID,
                Name = "UpdatedEntity",
                CreatedOn = category.CreatedOn
            };
            using (var repo1 = RepositoryContext.GetRepository())
            {
                repo1.Update(DTO_T0_Entity_category);
                repo1.SaveChanges();
                repo1.Dispose();
            }

            using (var repo2 = RepositoryContext.GetRepository())
            {
                var result = repo2.GetByID<Category>(category.ID);
                Assert.AreEqual(result.Name, DTO_T0_Entity_category.Name);
            }
        }

        /// <summary>
        ///  DTO 转换成Entity 执行更新操作
        /// </summary>
        [TestMethod()]
        public void DTO_Add_NormalTest()
        {
            var category = new Category() { Name = "DTO_Add_NormalTest" };
            using (var repo = RepositoryContext.GetRepository())
            {
                repo.Add(category);
                Assert.AreEqual(category.SysState, EntityStateEnum.Inserting);
                repo.SaveChanges();
            }

            using (var repo1 = RepositoryContext.GetRepository())
            {
                var result = repo1.GetByID<Category>(category.ID);
                Assert.AreEqual(result.Name, category.Name);
            }
        }

        #endregion

        #endregion

        #region 简单查询

        [TestMethod]
        public void GetByID_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "GetByID_Test" };
                repo.Add(category);
                repo.SaveChanges();
                var added = repo.GetByID<Category>(category.ID);
                Assert.IsNotNull(added);
                Assert.IsTrue(added.Name == "GetByID_Test");
            }
        }

        [TestMethod]
        public void GetAll_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "GetAll_Test" };
                repo.Add(category);
                repo.SaveChanges();
                var all = repo.GetAll<Category>();
                Assert.IsNotNull(all);
                Assert.IsTrue(all.Count > 0);
            }
        }



        #endregion

        #region 多库操作
        /// <summary>
        /// 测试finder之后修改属性，实体状态以及PL中实体状态
        /// </summary>
        [TestMethod()]
        public void Add_DemoTest()
        {
            var category = new Category() { Name = "Add_DemoTest" };
            using (var repo = RepositoryContext.GetRepository())
            {
                repo.Add(category);
                Assert.AreEqual(category.SysState, EntityStateEnum.Inserting);
                repo.SaveChanges();
            }
            using (var repo1 = RepositoryContext.GetRepository())
            {
                var added = repo1.GetByID<Category>(category.ID);
                Assert.IsNotNull(added);
                Assert.IsTrue(added.Name == category.Name);
            }
        }

        /// <summary>
        /// 测试多库设置下，对其中一个库进行 新增操作
        /// </summary>
        [TestMethod()]
        public void Add_DemoBTest()
        {
            var vehicle = new Vehicle() { Name = "Add_DemoBTest" };
            using (var repo = RepositoryContext.GetRepository())
            {
                repo.Add(vehicle);
                Assert.AreEqual(vehicle.SysState, EntityStateEnum.Inserting);
                repo.SaveChanges();
            }

            using (var repo1 = RepositoryContext.GetRepository())
            {
                var added = repo1.GetByID<Vehicle>(vehicle.ID);
                Assert.IsNotNull(added);
                Assert.IsTrue(added.Name == vehicle.Name);
            }

        }

        /// <summary>
        /// 测试多库设置下，对其中一个库进行 新增操作
        /// </summary>
        [TestMethod()]
        public void Add_SeparateDBTest()
        {
            var category = new Category() { Name = "Add_DemoTest" };
            var vehicle = new Vehicle() { Name = "Add_DemoBTest" };
            using (var repo = RepositoryContext.GetRepository())
            {
                repo.Add(category);

                repo.Add(vehicle);
                Assert.AreEqual(vehicle.SysState, EntityStateEnum.Inserting);
                repo.SaveChanges();
            }

            using (var repo1 = RepositoryContext.GetRepository())
            {
                var addedDemo = repo1.GetByID<Category>(category.ID);
                Assert.IsNotNull(addedDemo);
                Assert.IsTrue(addedDemo.Name == category.Name);

                var addedDemoB = repo1.GetByID<Vehicle>(vehicle.ID);
                Assert.IsNotNull(addedDemoB);
                Assert.IsTrue(addedDemoB.Name == vehicle.Name);
            }

        }
        #endregion

        #region 原生 sql 查询 测试
        /*[TestMethod]
        public void SqlQuery_Test()
        {
            using(var repo = RepositoryContext.GetRepository())
            {
               repo.SqlQuery<Category>("Select count(*) from CategorY");
            }
        }*/

        #endregion

        #region ExceptionTest
        [TestMethod]
        [ExpectedException(typeof(ConcurrentModificationException))]
        public void ParallellyModificationException_UpdateDeletedItemTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "ParallellyModificationException_Test" };
                repo.Add(category);
                repo.SaveChanges();
                var added = repo.GetByID<Category>(category.ID);
                repo.Remove(added);
                repo.SaveChanges();
                added.Size = 1;
                repo.Update(added);
                repo.SaveChanges();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ConcurrentModificationException))]
        public void ParallellyModificationException_DeleteDeltedItemTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() { Name = "ParallellyModificationException_DeleteDeltedItemTest" };
                repo.Add(category);
                repo.SaveChanges();
                var added = repo.GetByID<Category>(category.ID);
                repo.Remove(added);
                repo.SaveChanges();
                repo.Remove(added);
                repo.SaveChanges();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NonUniqueEntityException))]
        public void ParallellyModificationException_AddExistingItemTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var id = Guid.NewGuid();
                var category = new Category() { ID = id, Name = "ParallellyModificationException_AddExistingItemTest" };
                repo.Add(category);
                repo.SaveChanges();

                var category2 = new Category() { ID = id, Name = "ParallellyModificationException_AddExistingItemTest" };
                repo.Add(category2);
                repo.SaveChanges();
            }
        }

        #endregion



    }
}
