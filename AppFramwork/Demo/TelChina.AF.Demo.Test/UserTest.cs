using TelChina.AF.Demo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TelChina.AF.Util.TestUtil;

namespace TelChina.AF.Demo.Test
{
    
    
    /// <summary>
    ///这是 UserTest 的测试类，旨在
    ///包含所有 UserTest 单元测试
    ///</summary>
    [TestClass()]
    public class UserTest
    {


        private TestContext testContextInstance;
        /// <summary>
        /// 循环次数
        /// </summary>
        private static int ITERATION = 1000;
        /// <summary>
        ///获取或设置测试上下文，上下文提供
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            CodeTimer.Initialize();
        }
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///ToBE 的测试
        ///</summary>
        [TestMethod()]
        public void DTOToBECeshi()
        {
            User target = new User(); // TODO: 初始化为适当的值
            UserDTO dto = target.ReturnUserDTO(); // TODO: 初始化为适当的值
            //Order expected = null; // TODO: 初始化为适当的值
            User actual;
            actual = target.ToBE(dto);


            Assert.AreEqual(actual.Birthday, dto.Birthday);
            Assert.AreEqual(actual.ID, dto.ID);
            Assert.AreEqual(actual.CreatedBy, dto.CreatedBy);
            Assert.AreEqual(actual.CreatedOn, dto.CreatedOn);
        }
        /// <summary>
        ///ToBE 的测试
        ///</summary>
        [TestMethod()]
        public void AutoMapperTest()
        {
            User target = new User(); // TODO: 初始化为适当的值
            User user = target.ReturnUser(); // TODO: 初始化为适当的值
            UserDTO dto = target.TransitiontoDto(user);
        }


        /// <summary>
        ///ToDTO 的测试
        ///</summary>
        [TestMethod()]
        public void BEToDTOCeshi()
        {
            User target = new User(); // TODO: 初始化为适当的值
            User be = target.ReturnUser(); // TODO: 初始化为适当的值
            //Order expected = null; // TODO: 初始化为适当的值
            UserDTO actual;
            actual = target.ToDTO(be);


            Assert.AreEqual(actual.Birthday, be.Birthday);
            Assert.AreEqual(actual.ID, be.ID);
            Assert.AreEqual(actual.CreatedBy, be.CreatedBy);
            Assert.AreEqual(actual.CreatedOn, be.CreatedOn);
        }

        /// <summary>
        /// 批量测试
        /// </summary>
        [TestMethod]
        public void BatchTest()
        {
            CodeTimer.Time("转换" + ITERATION + "次TOBE通过映射能测试", 1, BatchDTOToBEFromUser);
            CodeTimer.Time("转换" + ITERATION + "次TODTO通过映射测试", 1, BatchDTOToDTOFromUser);
            CodeTimer.Time("转换" + ITERATION + "次TOBE通过一一赋值测试", 1, BatchDTOToBEFromUserDTO);
            CodeTimer.Time("转换" + ITERATION + "次TODTO通过一一赋值测试", 1, BatchDTOToDTOFromUserDTO);
            CodeTimer.Time("转换" + ITERATION + "次TODTO通过AutoMapper赋值", 1, BatchAutoToDTOMapper);
            CodeTimer.Time("转换" + ITERATION + "次TOBE通过AutoMapper赋值", 1, BatchAutoToBEMapper);
        }

        /// <summary>
        /// 转换多次TOBE通过映射能测试
        /// </summary>
        [TestMethod]
        public void BatchDTOToBEFromUserTest()
        {
            CodeTimer.Time("转换" + ITERATION + "次TOBE通过映射能测试", 1, BatchDTOToBEFromUser);
        }

        /// <summary>
        /// 转换多次TODTO通过映射测试
        /// </summary>
        [TestMethod]
        public void BatchDTOToDTOFromUserTest()
        {
            CodeTimer.Time("转换" + ITERATION + "次TODTO通过映射测试", 1, BatchDTOToDTOFromUser);
        }

        /// <summary>
        /// 转换多次TOBE通过一一赋值测试
        /// </summary>
        [TestMethod]
        public void BatchDTOToBEFromUserDTOTest()
        {
            CodeTimer.Time("转换" + ITERATION + "次TOBE通过一一赋值测试", 1, BatchDTOToBEFromUserDTO);
        }

        /// <summary>
        /// 转换多次TODTO通过一一赋值测试
        /// </summary>
        [TestMethod]
        public void BatchDTOToDTOFromUserDTOTest()
        {
            CodeTimer.Time("转换" + ITERATION + "次TODTO通过一一赋值测试", 1, BatchDTOToDTOFromUserDTO);
        }

        /// <summary>
        /// 转换多次TODTO通过AutoMapper赋值
        /// </summary>
        [TestMethod]
        public void BatchAutoToDTOMapperTest()
        {
            CodeTimer.Time("转换" + ITERATION + "次TODTO通过AutoMapper赋值", 1, BatchAutoToDTOMapper);
        }
        /// <summary>
        /// 转换多次TOBE通过AutoMapper赋值
        /// </summary>
        [TestMethod]
        public void BatchAutoToBEMapperTest()
        {
            CodeTimer.Time("转换" + ITERATION + "次TOBE通过AutoMapper赋值", 1, BatchAutoToBEMapper);
        }

        /// <summary>
        /// 转换多次TOBE通过映射
        /// </summary>
        private void BatchDTOToBEFromUser()
        {
            for (int i = 0; i < ITERATION; i++)
            {
                User user = new User();
                UserDTO dto = user.ReturnUserDTO();
                user = user.ToBE(dto);
            }
        }

        /// <summary>
        /// 转换多次TODTO通过映射
        /// </summary>
        private void BatchDTOToDTOFromUser()
        {
            for (int i = 0; i < ITERATION; i++)
            {
                User user = new User();
                User be = user.ReturnUser();
                UserDTO dto = user.ToDTO(be);
            }
        }

        /// <summary>
        /// 转换多次TOBE通过一一赋值
        /// </summary>
        private void BatchDTOToBEFromUserDTO()
        {
            for (int i = 0; i < ITERATION; i++)
            {
                User user = new User();
                UserDTO dto = user.ReturnUserDTO();
                user = dto.ToBE(dto);
            }
        }

        /// <summary>
        /// 转换多次TODTO通过一一赋值
        /// </summary>
        private void BatchDTOToDTOFromUserDTO()
        {
            for (int i = 0; i < ITERATION; i++)
            {
                User user = new User();
                User be = user.ReturnUser();
                UserDTO userDTO = new UserDTO();
                UserDTO dto = userDTO.ToDTO(be);
            }
        }

        /// <summary>
        /// 转换多次AutoMapper赋值
        /// </summary>
        private void BatchAutoToDTOMapper()
        {
            for (int i = 0; i < ITERATION; i++)
            {
                User target = new User(); // TODO: 初始化为适当的值
                User user = target.ReturnUser(); // TODO: 初始化为适当的值
                UserDTO dto = target.TransitiontoDto(user);
            }
        }

        /// <summary>
        /// 转换多次AutoMapper赋值
        /// </summary>
        private void BatchAutoToBEMapper()
        {
            for (int i = 0; i < ITERATION; i++)
            {
                User target = new User(); // TODO: 初始化为适当的值
                UserDTO dto = target.ReturnUserDTO(); // TODO: 初始化为适当的值
                User be = target.TransitiontoBE(dto);
            }
        }
    }
}
