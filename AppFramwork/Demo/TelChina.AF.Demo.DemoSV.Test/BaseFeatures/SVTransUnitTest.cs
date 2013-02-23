using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Tool.hbm2ddl;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Service.AppHosting;
using TelChina.AF.Util.Common;
using System.Transactions;
using TelChina.AF.Util.Logging;
using TelChina.AF.Sys.Service;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using TelChina.AF.Persistant;
using TelChina.AF.Demo;
using TelChina.AF.Util.TestUtil;
using TelChina.AF.Demo.DemoSV.Exceptions;
using TelChina.AF.Sys.Context;
//using TelChina.AF.Demo.DomainModel.Model;
//using TelChina.AF.Domain.Core;


namespace TelChina.AF.Test.DemoSV.Test
{
    [TestClass]
    public class SVTransUnitTest
    {
        private IRepository _repository;
        private static ILogger logger = LogManager.GetLogger("SVTransUnitTest");
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //没有这句,impl.Dll不会自动复制到测试目录下,导致无法正确执行服务加载
            CodeTimer.Initialize();
            var sv = new TransSV();
            sv = null;
            logger.Debug("服务正在启动...");
            if ("Integrate" == ConfigHelper.GetConfigValue("DeployType"))
            {
                AppHost.Start();
                logger.Debug("服务已经启动");
            }
            RepositoryContext.Config();
        }

        [ClassCleanup]
        public static void MyClassCleanup()
        {
            logger.Debug("服务正在关闭...");
            if ("Integrate" == ConfigHelper.GetConfigValue("DeployType"))
            {
                AppHost.Stop();
                logger.Debug("服务已经结束");
            }
        }
        [TestInitialize]
        public void MyTestInitialize()
        {


            var context = GetProfileContext();
            ContextSession.Current = context;
            //CallContext.SetData(ServiceContextManager<ServiceContext>.SESSIONCOTEXTKEY, context);

            _repository = RepositoryContext.GetRepository();
            //清理垃圾数据
            var rubbish = _repository.GetAll<Category>();
            if (rubbish == null || rubbish.Count() <= 0) return;
            foreach (var item in rubbish)
                this._repository.Remove(item);

            _repository.SaveChanges();
        }
        [TestCleanup]
        public void MyTestClearup()
        {

        }
        private static ServiceContext GetProfileContext()
        {
            return new ServiceContext
            {
                LoginDate = DateTime.Now,
                UserCode = "admin",
                UserName = "admin",
                LoginIP = "127.0.0.1",
                UserID = "123",
                Token = Guid.NewGuid().ToString(),
                Content = new Dictionary<string, string>() { { "ContentKey1", "ContentValue1" }, { "ContentKey2", "ContentValue2" } }

            };
        }
        #region SingleRequiredSV


        /// <summary>
        /// 事务范围内的Required服务,正常提交的场景
        /// 期望的结果是:在外部提交后事务正常提交
        /// </summary>
        [TestMethod]
        public void SingleRequireSVTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
                                               {
                                                   logger.Debug("ThreadName&ID:" + AppDomain.CurrentDomain.FriendlyName + "*" +
                                                       Thread.CurrentThread.Name + Thread.CurrentThread.ManagedThreadId);

                                                   //CallContext.SetData("ThreadName", Thread.CurrentThread.Name);
                                                   CallContext.LogicalSetData("ThreadName", Thread.CurrentThread.Name);

                                                   var sv = ServiceProxy.CreateProxy<ITransSV>();
                                                   return sv.Required(true);
                                               }, true);
            var entity = _repository.GetByID<Category>(result);
            Assert.IsNotNull(entity);

        }

        [TestMethod]
        public void SingleRequireSVNewProxyTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                logger.Debug("ThreadName&ID:" + AppDomain.CurrentDomain.FriendlyName + "*" +
                    Thread.CurrentThread.Name + Thread.CurrentThread.ManagedThreadId);

                //CallContext.SetData("ThreadName", Thread.CurrentThread.Name);
                CallContext.LogicalSetData("ThreadName", Thread.CurrentThread.Name);

                var sv = ServiceProxy.CreateProxy<ITransSV>();
                return sv.Required(true);
            }, true);
            var entity = _repository.GetByID<Category>(result);
            Assert.IsNotNull(entity);
        }
        [TestMethod]
        public void SingleRequireSVNewProxyMultiCallTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                logger.Debug("ThreadName&ID:" + AppDomain.CurrentDomain.FriendlyName + "*" +
                    Thread.CurrentThread.Name + Thread.CurrentThread.ManagedThreadId);

                //CallContext.SetData("ThreadName", Thread.CurrentThread.Name);
                CallContext.LogicalSetData("ThreadName", Thread.CurrentThread.Name);

                var sv = ServiceProxy.CreateProxy<ITransSV>();
                sv.Required(true);

                return sv.Required(true);
            }, true);
            var entity = _repository.GetByID<Category>(result);
            Assert.IsNotNull(entity);
        }
        /// <summary>
        /// 外部没有事务的Required服务,正常提交的场景
        /// 期望的结果是:服务内创建事务并提交
        /// 根据实验,发现事务没有提升到DTC级别,处于本地LTM轻量级事务状态
        /// </summary>
        [TestMethod]
        public void SingleRequireSVWithoutOuterTransTest()
        {
            Guid result = Guid.Empty;
            //using (var trans = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(1, 1, 1, 0)))
            //{
            Console.Write("服务执行前的事务属性:");
            //LogTransactionInfo(Transaction.Current);
            var sv = new TransSVAgent();
            result = sv.Required(true);
            Console.Write("服务完成后事务提交前的事务属性:");
            //LogTransactionInfo(Transaction.Current);
            //    trans.Complete();
            //}
            var entity = _repository.GetByID<Category>(result);
            Assert.IsNotNull(entity);
        }
        /// <summary>
        /// 事务范围内的Required服务,需要回滚的场景
        /// 期望的结果是:在外部回滚后事务正常回滚
        /// </summary>
        [TestMethod]
        public void SingleRequireSVDoNotCommitTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                var sv = new TransSVAgent();
                return sv.Required(true);
            }, false);
            //using (var trans = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(1, 1, 1, 0)))
            //{
            //    Console.Write("服务执行前的事务属性:");
            //    //LogTransactionInfo(Transaction.Current);
            //    var sv = new TransSVAgent();
            //    result = sv.Required(true);
            //    Console.Write("服务完成后事务提交前的事务属性:");
            //    //LogTransactionInfo(Transaction.Current);
            //}
            var entity = _repository.GetByID<Category>(result);
            Assert.IsNull(entity);
        }
        //ToDO 需要处理好异常的传输
        [ExpectedExceptionAttribute(typeof(ExpectedException))]
        [TestMethod]
        public void SingleRequireSVFailedTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                var sv = new TransSVAgent();
                return sv.Required(false);
            }, true);

            //using (var trans = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(1, 1, 1, 0)))
            //{
            //    Console.Write("服务执行前的事务属性:");
            //    //LogTransactionInfo(Transaction.Current);
            //    var sv = new TransSVAgent();
            //    result = sv.Required(false);
            //    Console.Write("服务完成后事务提交前的事务属性:");
            //    //LogTransactionInfo(Transaction.Current);
            //}
        }

        #endregion SingleRequiredSV

        #region RequriesNewSV
        /// <summary>
        /// 事务范围内的RequiresNew服务,正常提交的场景
        /// 期望的结果是:在服务内部创建出新的事务边界,并提交
        /// 外部事务的提交与否对内部事务无影响
        /// </summary>
        [TestMethod]
        public void RequriesNewSVTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                var sv = new TransSVAgent();
                return sv.RequiresNew(true);
            }, true, true,
                trans =>
                    Assert.AreEqual(trans.TransactionInformation.Status,
                    TransactionStatus.Active,
                    "外部事务受到内部事务的影响,无法正确提交")
            );

            var entity = _repository.GetByID<Category>(result);
            Assert.IsNotNull(entity);

            result = TransInvoke.InvokTransFunction(() =>
            {
                var sv = new TransSVAgent();
                return sv.RequiresNew(true);
            }, false, true,
                 trans =>
                        Assert.AreEqual(trans.TransactionInformation.Status,
                        TransactionStatus.Active,
                        "外部事务受到内部事务的影响,无法正确提交"));
            entity = _repository.GetByID<Category>(result);
            Assert.IsNotNull(entity, "由于外部事务的回滚导致内部事务的回滚");
        }

        [TestMethod]
        public void RequriesNewWithOutOuterTransTest()
        {

            var sv = new TransSVAgent();
            var result = sv.RequiresNew(true);

            var entity = _repository.GetByID<Category>(result);
            Assert.IsNotNull(entity, "由于外部事务的回滚导致内部事务的回滚");
        }

        /// <summary>
        /// 事务范围内的RequiresNew服务,服务发生异常
        /// 期望的结果是:数据回滚但不影响外部事务
        /// </summary>
        [TestMethod]
        public void RequriesNewSVFailedTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                var sv = new TransSVAgent();
                return sv.RequiresNew(false);
            }, true, false,
            trans =>
                Assert.AreEqual(trans.TransactionInformation.Status,
                TransactionStatus.Active,
                "外部事务受到内部事务的影响,无法正确提交")
            );
        }
        #endregion

        #region NotSuppoted
        [TestMethod]
        public void NotSuppotedSVTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                var sv = new TransSVAgent();
                return sv.NotSupported(true);
            }, true, true,
            trans =>
                Assert.AreEqual(trans.TransactionInformation.Status,
                TransactionStatus.Active,
                "不支持事务的服务模式,外部事务与内部事务环境进发生了交互")
            );
        }
        [TestMethod]
        public void NotSuppotedFaildSVTest()
        {
            var result = TransInvoke.InvokTransFunction(() =>
            {
                var sv = new TransSVAgent();
                return sv.NotSupported(false);
            }, true, false,
            trans =>
                Assert.AreEqual(trans.TransactionInformation.Status,
                TransactionStatus.Active,
                "不支持事务的服务模式,外部事务与内部事务环境进发生了交互")
            );
        }
        #endregion NotSuppoted
    }
}
