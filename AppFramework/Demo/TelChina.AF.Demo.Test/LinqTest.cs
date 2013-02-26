using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;
using TelChina.AF.Demo;
using System.Linq.Expressions;

namespace TelChina.AF.Demo.Test
{
    [TestClass]
    public class LinqTest
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            RepositoryContext.Config();
        }

        //  Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                foreach (var item in repo.GetAll<Category>())
                {
                    repo.Remove(item);
                }
                repo.SaveChanges();
            }   
        }

        #region Lamdba Test
        [TestMethod]
        public void GetByLambda_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                IList<Category> dbSortedCategories = repo.GetAll<Category>(T => T.Name != null, T => T.CreatedOn, false);
                IList<Category> allCategories = repo.GetAll<Category>();
                var memorySorted =
                    (from item in allCategories where item.Name != null orderby item.CreatedOn ascending select item).
                        ToList<Category>();

                Assert.AreEqual(dbSortedCategories.Count, allCategories.Count);
                for (int i = 0; i < dbSortedCategories.Count; i++)
                {
                    Assert.AreEqual(dbSortedCategories[i], memorySorted[i]);
                }
            }
        }

        [TestMethod]
        public void OrderByTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var result = repo.GetAll<Category>(T => T.Name != null, null, true);
                Assert.IsNotNull(result);
            }
        }
        #endregion

        #region Linq Test

        [TestMethod]
        public void LinqNormalTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() {Name = "LinqNormalTest", Description = "Description"};
                repo.Add(category);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              where item.Name != null && item.Description.IndexOf("ption") > 0 && item.ID == category.ID
                                    && item.CreatedOn <= DateTime.Now && item.CreatedOn.Year == 2012
                              orderby item.Name descending
                              orderby item.Description ascending
                              select item).ToList<Category>();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count == 1);
            }
        }

        #region 限制运算符
        [TestMethod]
        public void Linq_BetweenAnd_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var category = new Category()
                                   {
                                       Name = "Linq_BetweenAnd_OrderByTest",
                                       Description = "Linq_BetweenAnd_OrderByTest_Description"
                                   };
                repo.Add(category);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              where item.CreatedOn <= DateTime.Now && item.CreatedOn >= date
                              orderby item.Name descending
                              orderby item.Description ascending
                              select item).ToList<Category>();

                var addedCategory = result.Find(t => t.ID == category.ID);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                Assert.IsTrue(addedCategory.CreatedOn >= date && addedCategory.CreatedOn <= DateTime.Now);
            }
        }

        [TestMethod]
        public void Linq_Like_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var category = new Category()
                {
                    Name = "Linq_Like_Test",
                    Description = "Linq_Like_Test_Description"
                };
                repo.Add(category);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              where item.ID == category.ID && item.Name.ToUpper().IndexOf("_LIKE_TEST") > 0
                              select item).ToList<Category>();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count == 1);
                Assert.IsTrue(result[0].ID == category.ID);
            }
        }

        [TestMethod]
        public void Linq_IndexOf_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var category = new Category()
                                   {
                                       Name = "Linq_IndexOf_Test",
                                       Description = "Linq_IndexOf_Test_Description"
                                   };
                repo.Add(category);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              where item.ID == category.ID && item.Name.IndexOf("IndexOf_Test") > 0
                              select item).ToList<Category>();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count == 1);
                Assert.IsTrue(result[0].ID == category.ID);
            }
        }

        #endregion

        #region 投影运算符

        [TestMethod]
        public void Projection_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var category = new Category()
                                   {
                                       Name = "Projection_Test",
                                       Description = "Projection_Test"
                                   };
                repo.Add(category);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              select new {item.Name, item.Size, item.ID}).ToList();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count == 1);
                Assert.IsTrue(result[0].ID == category.ID);
            }
        }

        #endregion

        #region 分区运算符

        [TestMethod]
        public void Pagination_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {

                var date = DateTime.Now.AddHours(-1);
                var category1 = new Category()
                                    {
                                        Name = "Pagination_Test1",
                                        Description = "Pagination_Test",
                                        Size = 1
                                    };

                var category2 = new Category()
                                    {
                                        Name = "Pagination_Test2",
                                        Description = "Pagination_Test",
                                        Size = 2
                                    };
                var category3 = new Category()
                                    {
                                        Name = "Pagination_Test3",
                                        Description = "Pagination_Test",
                                        Size = 3
                                    };
                repo.Add(category1);
                repo.Add(category2);
                repo.Add(category3);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              orderby item.Size
                              select item).Take(2).Skip(1).ToList();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count == 2);
                Assert.IsTrue(result[0].Name == category2.Name);
                Assert.IsTrue(result[1].Name == category3.Name);
            }
        }

        #endregion

        #region 排序运算符

        [TestMethod]
        public void Linq_OrderBy_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var idString = "Linq_OrderBy_Test_Description" + DateTime.Now.ToString("yymmdd:hhMMss");
                var category1 = new Category()
                                    {
                                        Name = "Linq_OrderBy_Test",
                                        Description = idString,
                                        Size = 2
                                    };

                var category2 = new Category()
                                    {
                                        Name = "ALinq_OrderBy_Test",
                                        Description = idString,
                                        Size = 2
                                    };
                var category3 = new Category()
                                    {
                                        Name = "Linq_OrderBy_Test",
                                        Description = idString,
                                        Size = 1
                                    };
                var category4 = new Category()
                                    {
                                        Name = "ALinq_OrderBy_Test",
                                        Description = idString,
                                        Size = 1
                                    };
                repo.Add(category1);
                repo.Add(category2);
                repo.Add(category3);
                repo.Add(category4);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              where item.Description == idString
                              orderby item.Size descending
                              select item).ThenBy(p => p.Name).ToList<Category>();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count == 4);
                Assert.IsTrue(result[0].ID == category2.ID);
                Assert.IsTrue(result[1].ID == category1.ID);
                Assert.IsTrue(result[2].ID == category4.ID);
                Assert.IsTrue(result[3].ID == category3.ID);

                var result2 =
                    repo.Query<Category>().Where(T => T.Description == idString).OrderByDescending(T => T.Size).ThenBy(
                        T => T.Name).ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    Assert.IsTrue(result[i].ID == result2[i].ID);
                }
            }
        }

        #endregion

        #region 分组运算符

        [TestMethod]
        public void Linq_GroupBy_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var category = new Category()
                                   {
                                       Name = "Linq_GroupBy_Test",
                                       Description = "Linq_GroupBy_Test_Description",
                                   };

                var category1 = new Category()
                                    {
                                        Name = "Linq_GroupBy_Test",
                                        Description = "Linq_GroupBy_Test_Description",
                                        Size = 9
                                    };
                repo.Add(category);
                repo.Add(category1);
                repo.SaveChanges();
               
            
                var groupquery = (from item in repo.Query<Category>()
                                  group item by item.Name
                                  into g
                                  select new
                                             {
                                                 g.Key,
                                                 Age = g.Sum(p => p.Size)
                                             }).ToList();
                var groupquery2 = repo.Query<Category>().GroupBy(o => o.Name)
                    .Select(o => new {o.Key, Age = o.Sum(p => p.Size)}).ToList();


                Assert.IsNotNull(groupquery);
                Assert.IsTrue(groupquery.Count > 0);
                Assert.IsNotNull(groupquery2);
                Assert.IsTrue(groupquery2.Count > 0);
                Assert.IsTrue(groupquery.Count == groupquery2.Count);
                for (int i = 0; i < groupquery.Count; i++)
                {
                    Assert.IsTrue(groupquery[i].Age == groupquery2[i].Age);
                }
                
            }
        }

        #endregion

        #region 设置运算符 (不完美支持，不能指定Distinct的字段，默认是主键)

        [TestMethod]
        public void LinqDistinctTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() {Name = "LinqDistinctTest", Description = "Description"};
                var category2 = new Category() {Name = "LinqDistinctTest", Description = "Description"};
                repo.Add(category);
                repo.Add(category2);
                repo.SaveChanges();
                var result = (from item in repo.Query<Category>()
                              select item.Name).Distinct().ToList();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count == 1);
            }
        }

        #endregion

        #region 元素运算符

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Linq_Element_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var category = new Category()
                                   {
                                       Name = "Linq_Element_Test",
                                       Description = "Linq_Element_Test_Description",
                                   };

                var category1 = new Category()
                                    {
                                        Name = "Linq_Element_Test",
                                        Description = "Linq_Element_Test_Description",
                                    };
                repo.Add(category);
                repo.Add(category1);
                repo.SaveChanges();

                var firstquery = repo.Query<Category>().First(u => u.Name == "Linq_Element_Test");
                var firstOrDefaultquery = repo.Query<Category>().FirstOrDefault(u => u.Name == "Linq_Element_Test");

                var singlequery = repo.Query<Category>().Single(u => u.Name == "Linq_Element_Test");//此处抛出异常，因为Single方法只能根据条件返回一条数据 
                var singleOrDefaultquery = repo.Query<Category>().SingleOrDefault(u => u.Name == "Linq_Element_Test");
                Assert.IsTrue(firstquery.Name == "Linq_Element_Test");
                Assert.IsTrue(firstOrDefaultquery.Name == "Linq_Element_Test");

                repo.Remove(category);
                repo.SaveChanges();
                Assert.IsTrue(singleOrDefaultquery.Name == "Linq_Element_Test");
                Assert.IsTrue(singlequery.Name == "Linq_Element_Test");
            }


        }

        #endregion

        #region  限定运算符

        [TestMethod]
        public void LinqLimitationTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() {Name = "LinqLimitationTest", Description = "Description"};
                var category2 = new Category() {Name = "LinqLimitationTest2", Description = "Description"};
                repo.Add(category);
                repo.Add(category2);
                repo.SaveChanges();

                Assert.IsTrue(repo.Query<Category>().Any(p => p.Name == category.Name));
                Assert.IsTrue(repo.Query<Category>().Any(p => p.Name == category2.Name));
                Assert.IsNotNull(repo.Query<Category>().Any(p => p.Name == "NotExist"));
            }
        }
        #endregion

        #region 聚合运算符

        [TestMethod]
        public void Linq_Sum_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var category = new Category()
                                   {
                                       Name = "Linq_Sum_Test",
                                       Description = "Linq_Sum_Test_Description",
                                       Size = 9

                                   };
                var category2 = new Category()
                                    {
                                        Name = "Linq_Sum_Test",
                                        Description = "Linq_Sum_Test_Description",
                                        Size = 1
                                    };

                repo.Add(category);
                repo.Add(category2);
                repo.SaveChanges();

                var sum = repo.Query<Category>().Sum(P => P.Size);
                var count = repo.Query<Category>().Count();
                var min = repo.Query<Category>().Min(P => P.Size);
                var max = repo.Query<Category>().Max(P => P.Size);
                Assert.IsTrue(sum == (category.Size + category2.Size));
                Assert.IsTrue(count == 2);
                Assert.IsTrue(max == category.Size);
                Assert.IsTrue(min == category2.Size);

                var result = from item in repo.Query<Category>()
                             group item by item.Name
                             into ss
                             select new {A = ss.Key, TotalPrice = ss.Sum(P => P.Size)}.TotalPrice;
                Assert.IsTrue(sum == (category.Size + category2.Size));
            }
        }

        [TestMethod]
        public void Linq_MaxMin_Test()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var date = DateTime.Now.AddHours(-1);
                var idString = "Linq_MaxMin_Test" + DateTime.Now.ToString("yymmdd:hhMMss");
                var category1 = new Category()
                                    {
                                        Name = "Linq_MaxMin_Test",
                                        Description = idString,
                                        Size = 2
                                    };

                var category2 = new Category()
                                    {
                                        Name = "Linq_MaxMin_Test",
                                        Description = idString,
                                        Size = 2
                                    };
                var category3 = new Category()
                                    {
                                        Name = "Linq_MaxMin_Test",
                                        Description = idString,
                                        Size = 1
                                    };
                var category4 = new Category()
                                    {
                                        Name = "Linq_MaxMin_Test",
                                        Description = idString,
                                        Size = 1
                                    };
                repo.Add(category1);
                repo.Add(category2);
                repo.Add(category3);
                repo.Add(category4);
                repo.SaveChanges();
                var maxResult = (from item in repo.Query<Category>()
                                 where item.Description == idString
                                 select item.Size).Max();
                var minResult = (from item in repo.Query<Category>()
                                 where item.Description == idString
                                 select item.Size).Min();

                Assert.IsTrue(maxResult >= 2 && minResult <= 1);
            }
        }
        #endregion

        [TestMethod]
        [Ignore]
        public void LinqJoinTest()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var category = new Category() {Name = "LinqGroupTest", Description = "LinqGroupTest_Description"};
                repo.Add(category);
                repo.Add(new Product() {Name = "LinqGroupTest", Category = category});
                repo.SaveChanges();
                var result = (from ca in repo.Query<Category>()
                              join pr in repo.Query<Product>() on ca.ID equals pr.Category.ID into os
                              select new {ca, os}).ToList();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                Assert.IsTrue(result[0].ca.Name == category.Name);
            }
        }

        [TestMethod]
        [Ignore]
        public void Linq()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var product = repo.GetByID<Product>(Guid.Parse("20D2C61A-CEBC-40CE-9D11-2EE8E041147B"));
                var cartegory = product.Category;
                var pro = (from item in repo.Query<Product>()
                           where item.Category.ID == Guid.Parse("3E534492-D01F-426D-B07B-D9264285C487")
                           select item).ToList();
                var c = pro;
            }
        }

        #endregion
    }
}
