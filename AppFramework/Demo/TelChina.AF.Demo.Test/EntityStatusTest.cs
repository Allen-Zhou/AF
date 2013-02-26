using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Demo;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.Test
{
    /// <summary>
    /// Summary description for EntityStatusTest
    /// </summary>
    [TestClass]
    public class EntityStatusTest
    {
        public EntityStatusTest()
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
       //  Use TestInitialize to run code before running each test 
         [TestInitialize()]
         public void MyTestInitialize()
         {
             RepositoryContext.Config();
         }
        
       //  Use TestCleanup to run code after each test has run
         [TestCleanup()]
         public void MyTestCleanup()
         {
             //var repo = RepositoryContext.GetRepository();
             //foreach (var item in repo.GetAll<Category>())
             //{
             //    repo.Remove(item);
             //}
             //repo.SaveChanges();
         }

        //
        #endregion

        #region 状态转换测试
         /// <summary>
         /// 测试New，实体状态以及PL中实体状态
         /// </summary>
         [TestMethod()]
         public void Status_Test()
         {
             var repo = RepositoryContext.GetRepository();
             Category category = new Category();
             repo.Add(category);
             Assert.AreEqual(category.SysState, EntityStateEnum.Inserting);
             repo.Update(category);
             Assert.AreEqual(category.SysState, EntityStateEnum.Inserting);
             repo.Remove(category);
             Assert.AreEqual(category.SysState, EntityStateEnum.Deleting);
         }
   
         #endregion

      /*  #region Dispose 测试
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void Dispose_NormalTest()
        {
            var repo = RepositoryContext.GetRepository();
            var category = new Category() { Name = "Dispose_NormalTest" };
            repo.Add(category);
            repo.SaveChanges();

            var addedcategory = repo.GetByID<Category>(category.ID);
        }

        #endregion
*/
        #region Detach测试
        ///// <summary>
        ///// 测试new实体，未调用repo.add 之前状态始终是Unchanged
        ///// </summary>
        //[TestMethod()]
        //public void DetachedNewTest()
        //{
        //    var repo = RepositoryContext.GetRepository();
        //    Category category = new Category() { Name = "DetachedNewTest" };
        //    repo.Detach(category);
        //    Assert.AreEqual(category.SysState, EntityStateEnum.Unchanged);
        //}

        ///// <summary>
        ///// 测试new实体，新增，然后detach，提交
        ///// 期望结果： 新增的实体不能detach，这没有意义，
        ///// </summary>
        //[TestMethod()]
        //[ExpectedException(typeof(ApplicationException))]
        //public void DetachedAddedTest()
        //{
        //    var repo = RepositoryContext.GetRepository();
        //    var category = new Category() { Name = "DetachedAddedTest" };
        //    repo.Add(category);
        //    repo.Detach(category);
        //    repo.SaveChanges();
        //    var addedCategory = repo.GetByID<Category>(category.ID);
        //    Assert.AreEqual(category.SysState, EntityStateEnum.Detached);
        //    Assert.IsNull(addedCategory);
        //}

        ///// <summary>
        ///// 新增实体，然后detach，提交
        ///// 期望结果： 实体状态为Detached，且没有更新到数据库
        ///// </summary>
        //[TestMethod()]
        //public void DetachUpdatedTest()
        //{
        //    var repo = RepositoryContext.GetRepository();
        //    var category = new Category() { Name = "DetachUpdatedTest" };
        //    repo.Add(category);
        //    repo.SaveChanges();
        //    var addedCategory = repo.GetByID<Category>(category.ID);
        //    addedCategory.Name = "DetachUpdatedTest_Updated";
        //    repo.Update(addedCategory);
        //    repo.Detach(addedCategory);
        //    Assert.AreEqual(addedCategory.SysState, EntityStateEnum.Detached);
        //    repo.SaveChanges();
        //    var updatedCategory = repo.GetByID<Category>(category.ID);
        //    Assert.AreNotEqual(updatedCategory.Name , addedCategory.Name);
        //}

        ///// <summary>
        ///// 新增实体，提交，然后delete，detach，提交
        ///// 期望结果： 实体从数据库删除， detach操作无效
        ///// TODO:删除的实体无法Detach，是否能够Detach？
        ///// </summary>
        //[TestMethod()]
        //public void DetachDeletedTest()
        //{
        //    var repo = RepositoryContext.GetRepository();
        //    var category = new Category() { Name = "DetachDeletedTest" };
        //    repo.Add(category);
        //    repo.SaveChanges();
        //    var addedCategory = repo.GetByID<Category>(category.ID);
        //    repo.Remove(addedCategory);
        //    Assert.AreEqual(addedCategory.SysState, EntityStateEnum.Deleting);
        //    repo.Detach(addedCategory);
        //    Assert.AreEqual(addedCategory.SysState, EntityStateEnum.Deleting);
        //    repo.SaveChanges();
        //    Assert.AreEqual(addedCategory.SysState, EntityStateEnum.Deleted);
        //    var detachedCategory = repo.GetByID<Category>(category.ID);
        //    Assert.IsNull(detachedCategory);
        //}

        ///// <summary>
        ///// 新增实体，查询出该实体，detach该实体
        ///// 期望结果： 实体状态为Detached
        ///// </summary>
        //[TestMethod()]
        //public void DetachFindTest()
        //{
        //    var repo = RepositoryContext.GetRepository();
        //    var category = new Category() { Name = "DetachFindTest" };
        //    repo.Add(category);
        //    repo.SaveChanges();
        //    var addedCategory = repo.GetByID<Category>(category.ID);
        //    repo.Detach(addedCategory);
        //    Assert.AreEqual(addedCategory.SysState, EntityStateEnum.Detached);
        //}
        #endregion

        #region Attach测试

        ///// <summary>
        ///// add实体，detach，attach， 提交
        ///// 期望结果：实体提交成功
        ///// </summary>
        //[TestMethod()]
        //public void AttachAddedTest()
        //{
        //    var repo = RepositoryContext.GetRepository();
        //    var category = new Category() { Name = "AttachAddedTest" };
        //    repo.Add(category);
        //    repo.Detach(category);
        //    repo.Attach(category);
        //    repo.SaveChanges();
        //    var addedCategory = repo.GetByID<Category>(category.ID);
        //    Assert.IsNotNull(addedCategory);
        //    Assert.AreEqual(addedCategory.SysState, EntityStateEnum.Unchanged);
        //}

        ///// <summary>
        ///// 测试Unchanged状态实体，Detached 实体状态以及PL中是否存在
        ///// </summary>
        //[TestMethod()]
        //[Ignore()]
        //public void AttachUpdatedTest()
        //{
        //    var repo = RepositoryContext.GetRepository();
        //    var category = new Category() { Name = "AttachUpdatedTest" };
        //    repo.Add(category);
        //    repo.SaveChanges();
        //    var addedCategory = repo.GetByID<Category>(category.ID);
        //    addedCategory.Name = "AttachUpdatedTest_Updated";
        //    repo.Update(addedCategory);
        //    repo.Detach(addedCategory);
        //    repo.Attach(addedCategory);
        //    repo.SaveChanges();
        //    var firstAddedCategory =  repo.GetByID<Category>(category.ID);
        //    var attachedCategory = repo.GetByID<Category>(addedCategory.ID);
        //    Assert.IsNotNull(firstAddedCategory);
        //    Assert.IsNotNull(attachedCategory);
        //    Assert.AreNotEqual(firstAddedCategory, attachedCategory);
        //}

        #endregion

        ///// <summary>
        ///// 测试Unchanged状态实体，Detached 实体状态以及PL中是否存在
        ///// </summary>
        //[TestMethod()]
        //public void DetachedFindTest()
        //{
        //    /*
        //       实体=Finder
        //     * 实体.Detached
        //     * 断言  实体状态以及PL中是否存在
        //     */
        //    Category category = new Category();
        //    //category = category.GetPersonById(id);
        //    category.Detached();
        //    Assert.AreEqual(category.SysState, EntityStateEnum.Detached);
        //}

        ///// <summary>
        ///// 测试修改状态实体，Detached 实体状态以及PL中是否存在
        ///// </summary>
        //[TestMethod()]
        //public void DetachedUpdateTest()
        //{
        //    /*
        //        实体=Finder
        //      * 实体.属性=改变
        //      * 实体.Detached
        //      * 断言  实体状态以及PL中是否存在
        //      */
        //    Category category = new Category();
        //    //category = category.GetPersonById(id);
        //    category.Code = "567";
        //    category.Detached();
        //    Assert.AreEqual(category.SysState, EntityStateEnum.Detached);
        //}

        ///// <summary>
        ///// 测试删除状态的实体，Detached 实体状态以及PL中是否存在
        ///// </summary>
        //[TestMethod()]
        //public void DetachedDeletingTest()
        //{
        //    /*
        //       实体=Find
        //     * 实体.Deleting
        //     * 实体.Detached
        //     * 断言  实体状态以及PL中是否存在
        //     */
        //    Category category = new Category();
        //    //category = category.GetPersonById(id);
        //    // category.Remove();
        //    category.Detached();
        //    Assert.AreEqual(category.SysState, EntityStateEnum.Detached);
        //}
        //#endregion
    }
}
