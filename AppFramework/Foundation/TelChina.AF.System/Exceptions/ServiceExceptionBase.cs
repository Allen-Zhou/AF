﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Security;

namespace TelChina.TRF.System.Exceptions
{
    /// <summary>
    /// Domas系统异常的基类
    /// 职责:处理序列化与反序列化操作
    /// 子类使用过程中需要添加的信息都应放在Data中,
    /// 否则信息需要在子类中进行序列化与反序列化操作,
    /// 如果没有相应逻辑,此类信息将在传输过程中丢失
    /// </summary>
    [Serializable]
    //[KnownType(typeof(BPExecption))]
    //[KnownType(typeof(UnhandledException))]
    [KnownType(typeof(Exception))]
    [KnownType(typeof(Hashtable))]
    public class ServiceExceptionBase : Exception, ISerializable
    {
        #region Constructor

        public ServiceExceptionBase()
        {

        }
        public ServiceExceptionBase(string message)
            : base(message)
        {

        }
        public ServiceExceptionBase(Exception ex)
        {
            this.Detail = ex;
        }
        /// <summary>
        /// 用于反序列化处理内部信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ServiceExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.data = (Hashtable)info.GetValue("ExceptionData", typeof(Hashtable));
            this.Detail = (Exception)info.GetValue("Detail", typeof(Exception));
        }
        #endregion Constructor

        #region 序列化处理

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("ExceptionData", this.data, typeof(Hashtable));
            info.AddValue("Detail", this.Detail, typeof(Exception));
        }
        #endregion

        #region 属性
        private Hashtable data;
        /// <summary>
        /// 异常内部信息,需要使用可以序列化的对象,
        /// Exception的Data属性是ListDictionaryInternal类型,不可序列化,不能直接使用,
        /// 故使用Hashtable来覆盖
        /// </summary>
        public override IDictionary Data
        {
            get
            {
                if (data == null)
                {
                    this.data = new Hashtable(5);
                }
                return this.data;
            }
        }
        /// <summary>
        /// 异常消息格式
        /// 用于生成异常消息,其中的序号要与字段顺序一致
        /// </summary>
        protected string MessageFormat
        {
            get
            {
                if (this.Data["MessageFormat"] == null)
                    this.Data["MessageFormat"] = "";
                return this.Data["MessageFormat"].ToString();
            }
            set { this.Data["MessageFormat"] = value; }
        }
        /// <summary>
        /// 重写异常基类的异常消息属性
        /// </summary>
        public override string Message
        {
            get
            {
                if (OrderedProperties != null && OrderedProperties.Count() > 0)
                    return string.Format(this.MessageFormat, ProtertyValues);
                else
                    return this.MessageFormat;
            }
        }

        /// <summary>
        /// 字段值列表
        /// </summary>
        protected string[] ProtertyValues
        {
            get
            {
                if (this.OrderedProperties != null && OrderedProperties.Count() > 0)
                {
                    List<string> result = new List<string>(this.OrderedProperties.Count());
                    foreach (string propertyName in OrderedProperties)
                    {
                        if (this.Data[propertyName] == null)
                            result.Add("");
                        else
                            result.Add(this.Data[propertyName].ToString());
                    }
                    return result.ToArray();
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// 用于WCF传输的原始异常
        /// </summary>
        public Exception Detail { get; set; }
        #endregion 属性

        /// <summary>
        /// 按照MessageFormat格式顺序来排列的属性字段名列表
        /// 用于生成异常Message
        /// </summary>
        protected IEnumerable<string> OrderedProperties { get; set; }
    }

}
