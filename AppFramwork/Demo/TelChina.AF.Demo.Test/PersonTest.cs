using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;
using TelChina.AF.Util.TestUtil;

namespace TelChina.AF.Demo.Test
{
    /// <summary>
    /// PersonTest 的摘要说明
    /// </summary>
    [TestClass]
    public class PersonTest
    {
        public PersonTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

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
        private Guid updateID = Guid.Parse("B2DD9318-7A40-4D44-A5C6-26DCA237E091");
        private Guid disabledID = Guid.Parse("995A7640-3303-47DF-AEC2-20E459CDFEC3");
        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            RepositoryContext.Config();
           
        }
        
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
           
        }
        
        //在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                Person person1 = Person.CreatePerson("Person-1005", "张三", false, Guid.Parse("B2DD9318-7A40-4D44-A5C6-26DCA237E091"));
                repo.Add(person1);
                Person person2 = Person.CreatePerson("Person-1006", "李四", false, Guid.Parse("91FBED4E-A754-4022-AE3D-7FF9C21861CC"));
                repo.Add(person2);
                Person person3 = Person.CreatePerson("Person-1007", "王五", true, Guid.Parse("995A7640-3303-47DF-AEC2-20E459CDFEC3"));
                repo.Add(person3);
                repo.SaveChanges();
            }
        }
        
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var personList = repo.GetAll<Person>();
                foreach (Person person in personList)
                {
                    if (person.ID == Guid.Parse("995A7640-3303-47DF-AEC2-20E459CDFEC3"))
                    {
                        person.Disabled = false;
                    }
                    repo.Remove(person);
                }
                repo.SaveChanges();
            }
        }
        
        #endregion
               

        /// <summary>
        /// EntityBase的SetDefaultValue
        /// </summary>
        [TestMethod()]
        public void EntityBaseSetDefaultValueTest()
        {
            Guid idperson;
            using (var repo = RepositoryContext.GetRepository())
            {
                var person = Person.CreatePerson("","",false,Guid.Empty);
                repo.Add(person);
                repo.SaveChanges();
                idperson = person.ID;
            }
            using (var repo = RepositoryContext.GetRepository())
            {
                var addedperson = RepositoryContext.GetRepository().GetByID<Person>(idperson);
                Assert.IsNotNull(addedperson);
                Assert.AreEqual(addedperson.CreatedBy, "DS");
                Assert.AreEqual(addedperson.UpdatedBy, "DS");
            }
        }
      
        /// <summary>
        /// EntityBase的SetDefaultValue
        /// </summary>
        [TestMethod()]
        public void PersonSetDefaultValueTest()
        {
            Guid idperson;
            using (var repo = RepositoryContext.GetRepository())
            {
                var person = Person.CreatePerson("", "", false, Guid.Empty);
                repo.Add(person);
                repo.SaveChanges();
                idperson = person.ID;
            }
            using (var repo = RepositoryContext.GetRepository())
            {
                var addedperson = RepositoryContext.GetRepository().GetByID<Person>(idperson);
                Assert.IsNotNull(addedperson);
                Assert.AreEqual(addedperson.Code, "Person-1111");
                Assert.AreEqual(addedperson.Name, "张三");
               
            }
        }
        /// <summary>
        /// Person的OnValidate
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void PersonOnValidateTest()
        {
            Guid idperson=Guid.NewGuid();

            try
            {
                using (var repo = RepositoryContext.GetRepository())
                {                    
                    var person = Person.CreatePerson("Person-010123", "", false, Guid.Empty);
                    idperson = person.ID;
                    repo.Add(person);
                }
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message.Trim(), "编码不能长于12位");
                using (var repo = RepositoryContext.GetRepository())
                {
                    var newperson = repo.GetByID<Person>(idperson);
                    Assert.IsNull(newperson);
                }
                throw;
            }
            
        }

        /// <summary>
        /// EntityBase的OnValidate
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void EntityBaseOnValidateTest()
        {
            Guid idperson = Guid.NewGuid();

            try
            {
                using (var repo = RepositoryContext.GetRepository())
                {
                    var person = Person.CreatePerson("Person-0101234567", "", false, Guid.Empty);
                    idperson = person.ID;
                    repo.Add(person);
                }
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message.Trim(), "字段超超长度");
                using (var repo = RepositoryContext.GetRepository())
                {
                    var newperson = repo.GetByID<Person>(idperson);
                    Assert.IsNull(newperson);
                }
                throw;
            }
            
        }

        /// <summary>
        /// Person的OnInserting
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void PersonOnInsertingTest()
        {
            Guid idperson = Guid.NewGuid();

            try
            {
                using (var repo = RepositoryContext.GetRepository())
                {
                    var person = Person.CreatePerson("Person-1171", "", false, Guid.Empty);
                    idperson = person.ID;
                    repo.Add(person);
                }
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message.Trim(), "编码存在，请重新输入！");
                using (var repo = RepositoryContext.GetRepository())
                {
                    var newperson = repo.GetByID<Person>(idperson);
                    Assert.IsNull(newperson);
                }
                throw;
            }

        }

        /// <summary>
        /// EntityBase的OnInserted
        /// </summary>
        [TestMethod()]
        public void EntityBaseOnInsertedTest()
        {
            Guid idperson;
            using (var repo = RepositoryContext.GetRepository())
            {
                var person = Person.CreatePerson("", "", false, Guid.Empty);
                repo.Add(person);
                repo.SaveChanges();
                Assert.AreEqual(person.SysState, EntityStateEnum.Unchanged);
                idperson = person.ID;
            }
            using (var repo = RepositoryContext.GetRepository())
            {
                var addedperson = RepositoryContext.GetRepository().GetByID<Person>(idperson);
                Assert.IsNotNull(addedperson);
            }
        }

        /// <summary>
        /// Person的OnUpdating
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void PersonOnUpdatingTest()
        {

            try
            {
                using (var repo = RepositoryContext.GetRepository())
                {
                    var person = repo.GetByID<Person>(updateID);
                    var telphone = person.Telphone.Substring(1, person.Telphone.Length - 1) + "1";
                    person.Telphone = telphone;
                    person.Code = "Person-1171";
                    repo.Update(person);
                }
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message.Trim(), "编码存在，请重新输入！");
                using (var repo = RepositoryContext.GetRepository())
                {
                    var updateperson = repo.GetByID<Person>(updateID);
                    Assert.AreEqual(updateperson.Code, "Person-1005");
                }
                throw;
            }
            
        }
       
        /// <summary>
        /// EntityBase的OnOnUpdated
        /// </summary>
        [TestMethod()]
        public void EntityBaseOnOnUpdatedTest()
        {
            Guid idperson = disabledID;
            using (var repo = RepositoryContext.GetRepository())
            {
                var person = repo.GetByID<Person>(idperson);
                person.Code = "Person-1007";
                repo.Update(person);
                repo.SaveChanges();
                Assert.AreEqual(person.SysState, EntityStateEnum.Unchanged);
                idperson = person.ID;
            }
            using (var repo = RepositoryContext.GetRepository())
            {
                var person = repo.GetByID<Person>(idperson);
                Assert.AreEqual(person.Code, "Person-1007");
            }
        }

        /// <summary>
        /// Person的OnDeleting
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void PersonOnDeletingTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                try
                {
                    var person = repo.GetByID<Person>(disabledID);
                    repo.Remove(person);
                    repo.SaveChanges();
                }
                catch (Exception e)
                {
                    Assert.AreEqual(e.Message.Trim(),"该人员被停用不能删除，请先启用");
                    var deletePerson = repo.GetByID<Person>(disabledID);
                    Assert.IsNotNull(deletePerson);
                    throw;
                }

            }
        }

        /// <summary>
        /// EntityBase的OnDeleted
        /// </summary>
        [TestMethod()]
        public void EntityBaseOnDeletedTest()
        {
            Guid idperson;
            using (var repo = RepositoryContext.GetRepository())
            {
                var person = Person.CreatePerson("", "", false, Guid.Empty);
                repo.Add(person);
                repo.SaveChanges();
                Assert.AreEqual(person.SysState, EntityStateEnum.Unchanged);
                repo.Remove(person);
                repo.SaveChanges();
                Assert.AreEqual(person.SysState, EntityStateEnum.Deleted);
                idperson = person.ID;
            }
            using (var repo = RepositoryContext.GetRepository())
            {     
                var addedperson = RepositoryContext.GetRepository().GetByID<Person>(idperson);
                Assert.IsNull(addedperson);
            }
        }
       
        /// <summary>
        /// 同时修改
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(TelChina.AF.Sys.Exceptions.BusinessException))]
        public void UpdateSuperveneUpdateTest()
        {
            Person person;
            Person personSame;
            try
            {
                using (var repo = RepositoryContext.GetRepository())
                {
                    person = repo.GetByID<Person>(disabledID);
                }

                using (var repo = RepositoryContext.GetRepository())
                {
                    personSame = repo.GetByID<Person>(disabledID);
                }
                using (var reponew = RepositoryContext.GetRepository())
                {
                    person.Code = "Person-1008";
                    reponew.Update(person);
                    reponew.SaveChanges();
                }

                using (var reponew = RepositoryContext.GetRepository())
                {
                    personSame.Code = "Person-1009";
                    reponew.Update(personSame);
                    reponew.SaveChanges();
                }
                
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message.Trim(), "数据保存失败");
                
                using (var repo = RepositoryContext.GetRepository())
                {
                    var personnew = repo.GetByID<Person>(disabledID);
                     Assert.AreEqual(personnew.Code, "Person-1008");                    
                }
                throw;
            }
        }

        /// <summary>
        /// 删除同时修改
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(TelChina.AF.Sys.Exceptions.BusinessException))]
        public void DeleteSuperveneUpdateTest()
        {
            Person person;
            Person personSame;
            try
            {
                using (var repo = RepositoryContext.GetRepository())
                {
                    person = repo.GetByID<Person>(updateID);
                }

                using (var repo = RepositoryContext.GetRepository())
                {
                    personSame = repo.GetByID<Person>(updateID);
                }
                using (var reponew = RepositoryContext.GetRepository())
                {
                    reponew.Remove(person);
                    reponew.SaveChanges();
                }

                using (var reponew = RepositoryContext.GetRepository())
                {
                    personSame.Code = "Person-1008";
                    reponew.Update(personSame);
                    reponew.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message.Trim(), "数据保存失败");

                using (var repo = RepositoryContext.GetRepository())
                {
                    var personold = repo.GetByID<Person>(updateID);
                    Assert.IsNull(personold);                   
                }
                throw;
            }
        }


        /// <summary>
        /// 修改同时删除
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(TelChina.AF.Sys.Exceptions.BusinessException))]
        public void UpdateSuperveneDeleteTest()
        {
            Guid idperson = Guid.Parse("91FBED4E-A754-4022-AE3D-7FF9C21861CC");

            Person person;
            Person personSame;
            try
            {
                using (var repo = RepositoryContext.GetRepository())
                {
                    person = repo.GetByID<Person>(idperson);
                }

                using (var repo = RepositoryContext.GetRepository())
                {
                    personSame = repo.GetByID<Person>(idperson);
                }
                using (var reponew = RepositoryContext.GetRepository())
                {
                    person.Code = "Person-1008";
                    reponew.Update(person);
                    reponew.SaveChanges();
                }

                using (var reponew = RepositoryContext.GetRepository())
                {                    
                    reponew.Remove(personSame);
                    reponew.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message.Trim(), "数据保存失败");

                using (var repo = RepositoryContext.GetRepository())
                {
                    var personnew = repo.GetByID<Person>(idperson);
                    Assert.AreEqual(personnew.Code, "Person-1008");
                }
                throw;
            }
        }

    }
}
