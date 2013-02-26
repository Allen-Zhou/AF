using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Util.Logging;

namespace TelChina.AF.Test.DemoSV.Test
{
    [TestClass]
    public class Logtest
    {
        public void TestMethod1()
        {
            ILogger logger = LogManager.GetLogger("Logtest");
            logger.Debug("Test");
        }
    }
}
