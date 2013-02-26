using System;
using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Demo;
using TelChina.AF.Sys.Serialization;
using TelChina.AF.Sys.Service;
using TelChina.AF.Demo.DemoSV.Interfaces;
using TelChina.AF.Util.Logging;
using TelChina.AF.Util.TestUtil;
using TelChina.AF.Util.Common;
using TelChina.AF.Service.AppHosting;
using TelChina.AF.Persistant;
using System.IO;
using TelChina.AF.Sys.Context;

namespace TelChina.AF.Test.DemoSV.Test
{
    [TestClass]
    public class CommonCRUDSVTest
    {
        private static readonly ILogger Logger = LogManager.GetLogger("CommonCRUDSvTest");
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //testContext.

            //没有这句,impl.Dll不会自动复制到测试目录下,导致无法正确执行服务加载
            CodeTimer.Initialize();

            Logger.Debug("服务正在启动...");
            if ("Integrate" == ConfigHelper.GetConfigValue("DeployType"))
            {
                AppHost.Start();
                RepositoryContext.Config();
                Logger.Debug("服务已经启动");
            }
            else
            {
                AF.Sys.Configuration.AFConfigurationManager.Setup();
            }
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
            TestHelper.ClearEntity<Answer>();
        }

        //[TestMethod]
        public void CUDTest()
        {
            var sv = ServiceProxy.CreateProxy<ICommonCRUDService>();
            var answer1 = CreateDTO();
            var answer2 = CreateDTO();
            answer2.SysState = EntityStateEnum.Deleting;
            answer2.Name = "Where did I Come from?";
            sv.Add(new List<Object>() { answer1, answer2 });

        }
        [TestMethod]
        public void CUDWithConverterTest()
        {
            var sv = ServiceProxy.CreateProxy<ICommonCRUDService>();
            var answer1 = CreateDTO();
            var answer2 = CreateDTO();
            //answer2.SysState = EntityStateEnum.Deleting;
            answer2.Name = "Where did I Come from?";
            sv.Save(new List<Object>() { answer1, answer2 });

            var key1 = new EntityKey() { EntityType = typeof(Answer).FullName, ID = answer1.ID };
            var key2 = new EntityKey() { EntityType = typeof(Answer).FullName, ID = answer2.ID };

            using (var repo = RepositoryContext.GetRepository())
            {
                var result1 = repo.GetByKey(key1) as Answer;

                Assert.IsNotNull(result1);
                Assert.AreEqual(answer1.ID, result1.ID);
                Assert.AreEqual(answer1.Name, result1.Name);
                Assert.AreEqual(EntityStateEnum.Unchanged, result1.SysState);
                Assert.AreEqual(answer1.SysVersion + 1, result1.SysVersion);
                //Assert.AreEqual(answer1.CreatedOn, result1.CreatedOn);
            }
        }

        [TestMethod]
        public void SerializeTest()
        {
            var srcDTO = CreateDTO();
            var srcEntity = CreateEntity();
            srcDTO.Question_ID = Guid.NewGuid();

            var buffer = new StringBuilder();
            var buffer2 = new StringBuilder();

            var serializer2 =
               new DataContractSerializer(typeof(Object), null,
                   int.MaxValue, false, true, null,
                   new EntityDTODataContractResolver());

            var result2 = DoSerialize(serializer2, buffer, srcDTO);
            var destEntity = result2 as Answer;

            Assert.IsNotNull(destEntity);
            //验证子类属性的序列化
            Assert.AreEqual(srcDTO.Name, destEntity.Name);
            //验证基类属性的序列化
            Assert.AreEqual(srcDTO.ID, destEntity.ID);
            Assert.AreEqual(srcDTO.CreatedOn, destEntity.CreatedOn);
            Assert.AreEqual(srcDTO.SysVersion, destEntity.SysVersion);
            //自定义枚举类型
            Assert.AreEqual(srcDTO.SysState, destEntity.SysState);
        }
        [TestMethod]
        public void DefaultDTODataContractSerializeTest()
        {
            var srcDTO = CreateDTO();

            var buffer = new StringBuilder();
            var serializer =
                new DataContractSerializer(typeof(Object), new List<Type>() { typeof(Answer), typeof(AnswerDTO) },
                int.MaxValue, false, true, null);

            var result = DoSerialize(serializer, buffer, srcDTO);
            var destDTO = result as AnswerDTO;

            Assert.IsNotNull(destDTO);
            Assert.AreNotEqual(srcDTO, destDTO);
            Assert.AreEqual(srcDTO.ID, destDTO.ID);
            Assert.AreEqual(srcDTO.Name, destDTO.Name);
            Assert.AreEqual(srcDTO.SysVersion, destDTO.SysVersion);
        }
        [TestMethod]
        public void DefaultEntityDataContractSerializeTest()
        {
            var srcEntity = CreateEntity();

            var buffer = new StringBuilder();
            var serializer =
                new DataContractSerializer(typeof(Object), new List<Type>() { typeof(Answer), typeof(AnswerDTO) },
                int.MaxValue, false, true, null);

            var result = DoSerialize(serializer, buffer, srcEntity);
            var destDTO = result as Answer;

            Assert.IsNotNull(destDTO);
            Assert.AreNotEqual(srcEntity, destDTO);
            Assert.AreEqual(srcEntity.ID, destDTO.ID);
            Assert.AreEqual(srcEntity.Name, destDTO.Name);
            Assert.AreEqual(srcEntity.SysVersion, destDTO.SysVersion);
        }
        [TestMethod]
        public void PerfomanceCompareBetween2Ways()
        {
            for (int i = 100; i <= 1000; i *= 10)
            {
                CodeTimer.Initialize();
                CodeTimer.Time(string.Format("序列化转换方式，执行次数{0}", i), i, CUDTest);
                //CodeTimer.Time("对应参数赋值转换方式", i, CUDWithConverterTest);
            }
        }

        #region HelperMethod

        private object DoSerialize(DataContractSerializer serializer, StringBuilder buffer, object srcObj)
        {
            Serialize(srcObj, buffer, serializer);
            var result = Deserialize(buffer, serializer);
            Assert.IsNotNull(result);
            return result;
        }

        private void Serialize(object instance, StringBuilder buffer,
            DataContractSerializer serializer)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(buffer))
            {
                try
                {
                    serializer.WriteObject(xmlWriter, instance);
                }
                catch (SerializationException error)
                {
                    Console.WriteLine(error.ToString());
                }
            }
            Console.WriteLine(buffer.ToString());
        }


        private object Deserialize(StringBuilder buffer,
            DataContractSerializer serializer)
        {
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(buffer.ToString())))
            {
                var obj = serializer.ReadObject(xmlReader);
                return obj;
            }
            //return null;
        }

        private static Answer CreateEntity()
        {
            var srcEntity = new Answer()
            {
                ID = Guid.NewGuid(),
                Name = "Who am I?",
                SysVersion = 0,
                SysState = EntityStateEnum.Inserting,
                CreatedOn = DateTime.Now
            };
            return srcEntity;
        }

        private static AnswerDTO CreateDTO()
        {
            var srcDTO = new AnswerDTO()
            {
                ID = Guid.NewGuid(),
                Name = "Who am I?",
                SysVersion = 0,
                SysState = EntityStateEnum.Inserting,
                CreatedOn = DateTime.Now
            };
            return srcDTO;
        }
        #endregion
    }
}
