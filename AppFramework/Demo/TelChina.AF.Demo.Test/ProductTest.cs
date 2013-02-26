using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.Test
{
    /// <summary>
    /// Summary description for ProductTest
    /// </summary>
    [TestClass]
    public class ProductTest
    {
        public ProductTest()
        {

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
         public static void MyClassInitialize(TestContext testContext)
         {
             RepositoryContext.Config();
         }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
           
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

        }

        #endregion

     

        #region Category

        /// <summary>
        /// 修改实体
        /// </summary>
        [TestMethod]
        public void CategoryProduct()
        {
            
        }
        #endregion

    }
}
