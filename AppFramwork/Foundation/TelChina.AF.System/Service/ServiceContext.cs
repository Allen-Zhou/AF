using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TelChina.AF.Sys.Service
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    //[Serializable]
    [DataContract]
    public class ServiceContext// : ISerializable
    {
        /// <summary>
        /// 用于反序列化的构造方法
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        //public ServiceContext(SerializationInfo info, StreamingContext context)
        //{

        //}
        //public ServiceContext()
        //{

        //}
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public string UserID
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 用户代码
        /// </summary>
        [DataMember]
        public string UserCode
        {
            get;
            set;
        }
        /// <summary>
        /// 登录日期
        /// </summary>
        [DataMember]
        public DateTime LoginDate
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端IP
        /// </summary>
        [DataMember]
        public string LoginIP
        {
            get;
            set;
        }
        /// <summary>
        /// 身份认证标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }
        /// <summary>
        /// 扩展内容
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Content { get; set; }

        ///// <summary>
        ///// 用于序列化的方法
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="context"></param>
        //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("ExceptionData", this.Content, typeof(Hashtable));
        //}
        public override string ToString()
        {
            var result = new StringBuilder(1024);
            result.Append(string.Format("UserID={0},", UserID));
            result.Append(string.Format("UserCode={0},", UserCode));
            result.Append(string.Format("UserName={0},", UserName));
            result.Append(string.Format("LoginDate={0},", LoginDate));
            result.Append(string.Format("LoginIP={0},", LoginIP));
            result.Append(string.Format("Token={0},", Token));

            result.Append(string.Format("Content=["));
            if (Content != null)
            {
                //必须ToList才能触发Lambda表达式的执行
                var stringResult = Content.Select(
                    item =>
                    {
                        return result.Append(
                            string.Format("(Key={0},Value={1})", item.Key, item.Value));
                    }).ToList();
            }
            else
            {
                result.Append("null");
            }
            result.Append("]");
            return result.ToString();
        }

    }
}
