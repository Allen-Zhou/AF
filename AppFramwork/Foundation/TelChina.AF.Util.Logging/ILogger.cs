using System;
using System.Collections.Generic;
using System.Text;

namespace TelChina.AF.Util.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// 调试信息-级别0
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);
        /// <summary>
        /// 日常信息-级别1
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);
        /// <summary>
        /// 业务异常-级别2
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);
        /// <summary>
        /// 系统异常-级别3
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);
        /// <summary>
        /// 系统异常-级别3
        /// 输出异常信息
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

    }
}
