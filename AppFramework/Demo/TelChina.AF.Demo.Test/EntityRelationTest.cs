using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.Test
{
    [TestClass]
    public class EntityRelationTest
    {
        public EntityRelationTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            RepositoryContext.Config();
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            #region 数据删除
            //using (var repo = RepositoryContext.GetRepository())
            //{
            //    var produts = repo.GetAll<Product>();
            //    if (produts != null && produts.Count > 0)
            //    {
            //        Array.ForEach(produts.ToArray(), repo.Remove);
            //    }

            //    var category = repo.GetAll<Category>();
            //    if (category != null && category.Count > 0)
            //    {
            //        Array.ForEach(category.ToArray(), repo.Remove);
            //    }
            //    repo.SaveChanges();
            //}
            #endregion
        }

        /// <summary>
        /// 一对多关系测试（父子关系）
        /// 包括插入测试，删除测试等
        /// </summary>
        [TestMethod]
        public void OneToManyTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var departmentA = new Department() { Name = "departmentTest0906" };
                var person1 = Person.CreatePerson("Person-1706", "张三", false,
                                                   Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E092"));
                var person2 = Person.CreatePerson("Person-1707", "王五", true,
                                                                 Guid.Parse("995A7641-3303-47DF-AEC2-20E459CDFEC4"));

                repo.Add(departmentA);
                departmentA.Persons.Add(person1);
                departmentA.Persons.Add(person2);
                repo.SaveChanges();

                var addeddepartment = RepositoryContext.GetRepository().GetByID<Department>(departmentA.ID);
                Assert.IsNotNull(addeddepartment);

                var addedperson = RepositoryContext.GetRepository().GetByID<Person>(person1.ID);
                Assert.IsNotNull(addedperson);

                addeddepartment.Persons.Remove(addedperson);
                repo.Update(addeddepartment);

                repo.SaveChanges();
            }
        }

        [TestMethod]
        public void OneToManyFKTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var departmentA = new Department() { Name = "departmentTestA" };
                var departmentB = new Department() { Name = "departmentTestB" };
                var person1 = Person.CreatePerson("Person-1005", "张三", false,
                                                  Guid.Parse("B2DD9318-7A40-4D44-A5C6-26DCA237E091"));
                var person2 = Person.CreatePerson("Person-1007", "王五", false,
                                                  Guid.Parse("995A7640-3303-47DF-AEC2-20E459CDFEC3"));
                departmentA.AddChild(person1);
                departmentA.AddChild(person2);

                repo.Add(departmentA);
                repo.Add(departmentB);

                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 测试一对多关系的懒加载与直接加载功能
        /// </summary>
        [TestMethod]
        public void LazyLoadTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var departments = repo.GetAll<Department>();
                foreach (var department in departments)
                {
                    if (department.Persons.Count > 0)
                    {
                        var name = department.Persons.FirstOrDefault().Name;
                    }
                }
            }
        }

        /// <summary>
        /// 组合关系添加测试
        /// </summary>
        [TestMethod]
        public void CompositionAddTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var product = new Product { Name = "Test_0918" };
                var category = new Category() { Name = "Ca_0918", Product = product };
                product.Category = category;
                //product.ID = Guid.NewGuid();
                //category.ID = product.ID;
                repo.Add(category);
                //repo.Add(product);
                repo.SaveChanges();
            }
            using (var repo = RepositoryContext.GetRepository())
            {
                var addCategory = (from item in repo.Query<Category>()
                                   where item.Name == "Ca_0918"
                                   select item).ToList<Category>();
                Assert.IsNotNull(addCategory);
                Assert.AreEqual(1, addCategory.Count);
                Assert.IsNotNull(addCategory[0].Product);
            }
        }

        /// <summary>
        /// 没有对应主键的一对一情况
        /// </summary>
        [TestMethod]
        public void CompositionChildrenTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var AddCategory = (from item in repo.Query<Category>()
                                   where item.Name == "Ca_0918"
                                   select item).ToList<Category>().FirstOrDefault();
                var product = AddCategory.Product;
                Assert.IsNull(product);
            }
        }

        /// <summary>
        /// 删除单个子类实例测试
        /// </summary>
        [TestMethod]
        public void ChildDeleteTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var department = (from item in repo.Query<Department>()
                                  where item.Name == "departmentTestA"
                                  select item).ToList<Department>().FirstOrDefault();
                Assert.IsNotNull(department);
                var addPerson1 = repo.GetAll<Person>(T => T.Name == "张三").FirstOrDefault();
                department.Persons.Remove(addPerson1);
                repo.Remove(addPerson1);
                repo.SaveChanges();
            }
        }

        [TestMethod]
        public void OneToOneTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                //var addPerson1 = repo.GetAll<Person>(T => T.Name == "李四").FirstOrDefault();
                var addedCategory = repo.GetAll<Category>(T => T.Name == "Ca_0918").FirstOrDefault();
                Assert.IsNotNull(addedCategory);
                var product = addedCategory.Product;
                Assert.IsNotNull(product);
            }
        }

        /// <summary>
        /// 实体组合关系测试
        /// </summary>
        [TestMethod]
        public void EntityCompositionTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var name = new Name() {First = "first_0920",Last = "last_0920"};
                var user = new User() {Name = name,Birthday = System.DateTime.Now};
                repo.Add(user);
                repo.SaveChanges();
            }
        }
        #region 继承关系测试
        /// <summary>
        /// 实体继承关系插入测试
        /// </summary>
        [TestMethod]
        public void EntityInheritInsertTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var childOne = new ChildOne() 
                { 
                    ID = Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E093"), 
                    FirstName = "clildOneFN_0927", 
                    LastName = "clildOneLN_0927",
                    Email = "Test_0927@163.com"
                };
                var childTwo = new ChildTwo()
                {
                    ID = Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E091"),
                    FirstName = "clildOneFN_0927",
                    LastName = "clildOneLN_0927",
                    Number = "13305310001"                    
                };
                repo.Add(childOne);
                repo.Add(childTwo);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 实体继承关系select测试
        /// </summary>
        [TestMethod]
        public void EntityInheritSelectTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var childOneTest = repo.GetByID<Parent>(Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E093"));
                var childTwoTest = repo.GetByID<ChildTwo>(Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E091"));
                Assert.AreEqual("Test_0927@163.com",((ChildOne) childOneTest).Email);
                Assert.AreEqual("13305310001", childTwoTest.Number);
            }
        }

        /// <summary>
        /// 实体继承关系删除测试
        /// </summary>
        [TestMethod]
        public void EntityInheritDeleteTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var child = repo.GetByID<Parent>(Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E091"));
                repo.Remove(child);
                repo.SaveChanges();
                var childDelete = repo.GetByID<Parent>(Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E091"));
                Assert.IsNull(childDelete);
            }
        }

        /// <summary>
        /// 实体继承关系更新测试
        /// </summary>
        [TestMethod]
        public void EntityInheritUpdateTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var child = repo.GetByID<ChildTwo>(Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E091"));
                child.Number = "1234567";
                repo.Update(child);
                repo.SaveChanges();
                var childUpdate = repo.GetByID<ChildTwo>(Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E091"));
                Assert.AreEqual("1234567",childUpdate.Number);
            }
        }

        /// <summary>
        /// 单独插入父类实体测试
        /// </summary>
        [TestMethod]
        public void EntityInheritParentInsertTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                //var parent= new Parent()
                //                   {
                //                       ID = Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E096"),
                //                       FirstName = "clildOneFN_0928",
                //                       LastName = "clildOneLN_0928"
                //                   };
                //repo.Add(parent);
                //repo.SaveChanges();
                //var parentInsert = repo.GetByID<Parent>(Guid.Parse("B2DD9317-7A40-4D44-A5C6-26DCA237E096"));
                //Assert.AreEqual("clildOneLN_0928",parentInsert.LastName);
            }
        }

        /// <summary>
        /// 单表实体父类测试，上级部门插入测试        
        /// </summary>
        [TestMethod]
        public void SoloEntityParentInsertTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var departmentA = new Department() { Name = "departmentTestA1015" ,Disabled = false};
                var departmentB = new Department() { Name = "departmentTestB1015" };
                var departmentC = new Department() { Name = "departmentTestC1015" };

                departmentB.Parent = departmentA;
                departmentC.Parent = departmentA;

                repo.Add(departmentA);
                repo.Add(departmentB);
                repo.Add(departmentC);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 单表实体父类测试，上级部门查询测试        
        /// </summary>
        [TestMethod]
        public void SoloEntityParentSelectTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addDepartmentTestB = repo.GetAll<Department>(T => T.Name == "departmentTestB1015").FirstOrDefault();
                var addParentDepartment = addDepartmentTestB.Parent;
                Assert.IsNotNull(addParentDepartment);
                Assert.AreEqual("departmentTestA1015",addParentDepartment.Name);
            }
        }

        /// <summary>
        /// 由于是单向关联删除上级部门并不能一起删掉下级部门
        /// </summary>
        [TestMethod]
        public void SoloEntityParentDeleteTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addDepartmentTestA = repo.GetAll<Department>(T => T.Name == "departmentTestA1015").FirstOrDefault();
                repo.Remove(addDepartmentTestA);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 单独删除子实体
        /// </summary>
        [TestMethod]
        public void SoloEntityChildDeleteTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addDepartmentTestB = repo.GetAll<Department>(T => T.Name == "departmentTestB1015").FirstOrDefault();
                repo.Remove(addDepartmentTestB);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 单独添加子实体测试
        /// </summary>
        [TestMethod]
        public void SoloEntityChildAddTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addDepartmentTestA = repo.GetAll<Department>(T => T.Name == "departmentTestA1015").FirstOrDefault();
                var addDepartmentTestD = new Department() { Name = "departmentTestD1016" };
                //addDepartmentTestB.idParent = addDepartmentTestA.ID;
                addDepartmentTestD.Parent = addDepartmentTestA;
                repo.Add(addDepartmentTestD);
                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 单实体双向关系测试
        /// </summary>
        [TestMethod]
        public void EntityParentChildAddTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var departmentA = new Department() { Name = "departmentTest1017", Code = "departmentTest1017" };
                var departmentChildA = new Department() { Name = "ChildA1017", Code = "ChildA1017" };
                var departmentChildB = new Department() { Name = "ChildB1017", Code = "ChildB1017" };

                departmentA.ChildrenDepartment.Add(departmentChildA);
                departmentA.ChildrenDepartment.Add(departmentChildB);

                repo.Add(departmentA);
                repo.Add(departmentChildA);
                repo.Add(departmentChildB);
                repo.SaveChanges();
            }
        }

        [TestMethod]
        public void EntityPKUpdateTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                
            }
        }

        #endregion
    }
}
