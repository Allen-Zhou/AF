using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.AF.Tool.Checked
{
    public class CheckModelDispather
    {
        /// <summary>
        /// 私有成员，抽象仓储工厂对象引用，指向具体实现的类的实例
        /// </summary>
        private readonly ValidateFactory _repositoryFactory;

        /// <summary>
        /// 构造函数，持有仓储工厂的引用，引用指向具体仓储工厂子类的实例
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public CheckModelDispather(ValidateFactory repositoryFactory)
        {
            this._repositoryFactory = repositoryFactory;
        }
    }
}
