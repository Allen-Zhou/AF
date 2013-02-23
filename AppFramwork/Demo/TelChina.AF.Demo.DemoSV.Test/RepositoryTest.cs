using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Demo;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Persistant;
using TelChina.AF.Sys.Service;

namespace TelChina.AF.Test.DemoSV.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            RepositoryContext.Config();
        }

        [TestMethod]
        public void CleanRepository_Test()
        {
            SaveCategory();

            using (var repo = RepositoryContext.GetRepository())
            {
                var category = repo.GetAll<Category>(T => T.Name == "SaveA");
                Assert.IsNotNull(category);
            }
        }

        public void SaveCategory()
        {
            using (var repo = RepositoryContext.GetRepository())
            {   
                var a = new Category() { Name = "SaveA" };
                repo.Add(a);

                SaveCategoryB();

                repo.Add(new Category() { Name = "SaveC" });
                repo.SaveChanges();
            }
        }

        public void SaveCategoryB()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var b = new Category() { Name = "SaveB" };
                repo.Add(b);
                repo.SaveChanges();
            }
        }
    }
}
