using System;
using System.Collections;
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
    [TestClass]
    public class ProcedureTest
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            RepositoryContext.Config();
        }

        /// <summary>
        /// 测试OUTPUT的存储过程        
        /// </summary>
        [TestMethod]
        public void DeleteProcedure()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var addedcategory = repo.GetAll<Category>();
                var test = addedcategory[0];
                //var list = repo.TestProcedure<Category>();

            }
        }

        /// <summary>
        /// 用IBatIS存储过程做实例
        /// 多个参数，单个返回值，利用泛型
        /// </summary>
        [TestMethod]
        public void IBatisProcedure()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                Hashtable ht = new Hashtable();
                ht.Add("name", "Category-21");
                ht.Add("size",20);
                var result = repo.IBatisProcedure<Category>("IBatisProcedure", ht);
                Assert.AreEqual(result.Name, "Category-21");
                Assert.AreEqual(result.Size, 20);
            }
        }

        /// <summary>
        /// IBatiS存储过程
        /// 返回值为泛型类型的集合
        /// </summary>
        [TestMethod]
        public void ListIBatisPro()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var test = repo.ListBatisPro<Category>("IBatisProcedureTest_2", new Hashtable());
            }
        }
    }
}
