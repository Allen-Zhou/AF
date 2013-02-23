using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.IO;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Demo.DemoSV.Exceptions;
using TelChina.AF.Sys.Exceptions;

namespace TelChina.AF.Test.DemoSV.Test
{
    [TestClass]
    public class ExceptionSerializeUnitTest
    {
        [TestMethod]
        public void TestExceptionSerialize()
        {
            DemoSVExecption obj = new DemoSVExecption();
            obj.Containt = "BP异常";
            DataContractSerializer formater = new DataContractSerializer(typeof(DemoSVExecption));

            using (Stream stream = new MemoryStream())
            {
                formater.WriteObject(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                DemoSVExecption exception = (DemoSVExecption)formater.ReadObject(stream);
            }
        }
        [TestMethod]
        public void TestExpectedExceptionSerialize()
        {

            ExpectedException obj = new ExpectedException();
            ExceptionBase eb = new ExceptionBase(obj);

            DataContractSerializer formater = new DataContractSerializer(typeof(ExceptionBase));

            using (Stream stream = new MemoryStream())
            {
                formater.WriteObject(stream, eb);
                stream.Seek(0, SeekOrigin.Begin);
                ExceptionBase exception = (ExceptionBase)formater.ReadObject(stream);
            }
        }
    }
}
