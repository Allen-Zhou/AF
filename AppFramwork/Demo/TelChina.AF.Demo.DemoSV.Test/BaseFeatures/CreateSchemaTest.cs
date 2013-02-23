using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Tool.hbm2ddl;
using TelChina.AF.Persistant;
using TelChina.AF.Sys.Configuration;

namespace TelChina.AF.Test.DemoSV.Test
{
    [TestClass]
    public class CreateSchemaTest
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            AFConfigurationManager.Setup();
            RepositoryContext.Config();
        }
        [TestMethod]
        public void CreateSchema()
        {
            RepositoryContext.GetRepository();
            var nhConfig = new ConfigurationBuilder().Build();
            var schemaExport = new SchemaExport(nhConfig);
            schemaExport
                .SetOutputFile(@"db.sql")
                .Execute(false, false, false);
        }
    }
}
