using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;
using TelChina.AF.Sys.Context;
using TelChina.AF.Sys.Service;

namespace TelChina.AF.Test.DemoSV.Test
{
    internal class TestHelper
    {
        /// <summary>
        /// 清理实体
        /// </summary>
        /// <typeparam name="T">需要被清理的类型</typeparam>
        public static void ClearEntity<T>() where T : EntityBase
        {

            ContextSession.Current = GetProfileContext();
            //CallContext.SetData(ServiceContextManager<ServiceContext>.SESSIONCOTEXTKEY, context);

            using (var _repository = RepositoryContext.GetRepository())
            {
                //清理垃圾数据
                var rubbish = _repository.GetAll<T>();
                if (rubbish == null || !rubbish.Any()) return;
                foreach (var item in rubbish)
                    _repository.Remove(item);

                _repository.SaveChanges();
            }
        }

        /// <summary>
        /// 生成用于测试的登录信息
        /// </summary>
        /// <returns></returns>
        public static ServiceContext GetProfileContext()
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
    }
}
