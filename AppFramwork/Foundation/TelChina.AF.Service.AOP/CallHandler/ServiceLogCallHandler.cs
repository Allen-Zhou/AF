using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections.Specialized;
using TelChina.AF.Util.Logging;
using System.Text;

namespace TelChina.AF.Service.AOP.CallHandler
{
    /// <summary>
    /// 服务日志切面的实现
    /// 根据日志级别在执行服务实例前后加入对服务调用情况的日志记录功能.
    /// 需要记录调用上下文信息,调用入口参数信息和正确执行后的返回值信息
    /// </summary>
    [ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class ServiceLogCallHandler : ICallHandler
    {
        private ILogger logger = LogManager.GetLogger("ServiceAOP");
        protected string Message { get; set; }
        protected string ParameterName { get; set; }
        /// <summary>
        /// 构造函数，此处不可省略，否则会导致异常
        /// </summary>
        /// <param name="attributes">配置文件中所配置的参数</param>
        public ServiceLogCallHandler(NameValueCollection attributes)
        {
            Order = 0;
            //从配置文件中获取key，如不存在则指定默认key
            this.Message = String.IsNullOrEmpty(attributes["Message"]) ? "" : attributes["Message"];
            this.ParameterName = String.IsNullOrEmpty(attributes["ParameterName"]) ? "" : attributes["ParameterName"];
        }
        /// <summary>
        /// 构造函数，此构造函数是用于Attribute调用
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="parameterName">参数名</param>
        public ServiceLogCallHandler(string message, string parameterName)
        {
            Order = 0;
            this.Message = message;
            this.ParameterName = parameterName;
        }
        /// <summary>
        /// 切面业务无关的技术处理
        /// </summary>
        /// <param name="input"></param>
        /// <param name="getNext"></param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            //检查参数是否存在
            if (input == null) throw new ArgumentNullException("input");
            if (getNext == null) throw new ArgumentNullException("getNext");

            //开始拦截，此处可以根据需求编写具体业务逻辑代码
            logger.Debug(string.Format("执行的服务类型:{0},操作名称:{1},参数信息:[{2}]",
                input.Target.GetType().FullName, input.MethodBase.Name, GetArguments(input.Arguments)));

            //调用具体方法
            var result = getNext()(input, getNext);
            if (result != null && result.ReturnValue != null)
            {
                logger.Debug(string.Format("返回值信息:[{0}]", result.ReturnValue.ToString()));
            }
            //判断所拦截的方法返回值是否是bool类型，
            //如果是bool则判断返回值是否为false,false:表示调用不成功，则直接返回方法不记录日志
            //if (result.ReturnValue.GetType() == typeof(bool))
            //{
            //    if (Convert.ToBoolean(result.ReturnValue) == false)
            //    {
            //        return result;
            //    }
            //}
            //如果调用方法没有出现异常则记录操作日志
            //if (result.Exception == null)
            //{
            //    //操作附加消息，用于获取操作的记录相关标识
            //    var actionMessage = "";
            //    object para = null;
            //    //判断调用方法的主要参数名是否为空，不为空则从拦截的方法中获取参数对象
            //    if (String.IsNullOrEmpty(this.ParameterName) == false)
            //    {
            //        para = input.Inputs[this.ParameterName];
            //    }
            //    //判断参数对象是否为null，不为null时则获取参数标识
            //    //此处对应着具体参数的ToString方法，我已经在具体类中override了ToString方法
            //    if (para != null)
            //    {
            //        actionMessage = " 编号:[" + para.ToString() + "]";
            //    }

            //    ////插入操作日志
            //    //Database db = DBHelper.CreateDataBase();
            //    //StringBuilder sb = new StringBuilder();
            //    //sb.Append("insert into UserLog(StudentId,Message,LogDate) values(@StudentId,@Message,@LogDate);");
            //    //DbCommand cmd = db.GetSqlStringCommand(sb.ToString());
            //    //db.AddInParameter(cmd, "@StudentId", DbType.Int32, uid);
            //    //db.AddInParameter(cmd, "@Message", DbType.String, this.Message + actionMessage);
            //    //db.AddInParameter(cmd, "@LogDate", DbType.DateTime, DateTime.Now);
            //    //db.ExecuteNonQuery(cmd);
            //}
            //返回方法，拦截结束
            return result;
        }

        private string GetArguments(IParameterCollection iParameterCollection)
        {
            var result = new StringBuilder(1024);
            if (iParameterCollection != null && iParameterCollection.Count > 0)
            {
                var index = 0;
                foreach (var param in iParameterCollection)
                {
                    if (param != null)
                        result.Append(string.Format("参数[{0}]:类型[{1}];", index++, param.GetType().FullName));
                    else
                        result.Append(string.Format("参数[{0}]:Null;", index++));
                }
            }
            else
            {
                result.Append(string.Format("此方法无参数"));
            }
            return result.ToString();
        }

        public int Order { get; set; }
    }

}
