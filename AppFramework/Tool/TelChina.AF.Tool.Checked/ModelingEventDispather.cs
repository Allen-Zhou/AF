using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TelChina.AF.Tool.Checked
{
    public class ModelingEventDispather : IModelingEvent
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private Dictionary<string, IModelingEvent> modelingEvents
        {
            set;
            get;
            /*get { return CallContext.GetData(REPOSITORIES_CACHES) as Dictionary<string, IRepository>; }
            set { CallContext.SetData(REPOSITORIES_CACHES, value); }*/
        }
        /// <summary>
        /// 私有成员，抽象仓储工厂对象引用，指向具体实现的类的实例
        /// </summary>
        private readonly ValidateFactory _validateFactory;

        /// <summary>
        /// 构造函数，持有仓储工厂的引用，引用指向具体仓储工厂子类的实例
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public ModelingEventDispather(ValidateFactory validateFactory)
        {
            this._validateFactory = validateFactory;
        }


        public void Register(Microsoft.VisualStudio.Modeling.Store store)
        {

        }


        public void ValidateNames(UMLClassType umlType, Microsoft.VisualStudio.Modeling.Validation.ValidationContext context, object model)
        {

        }
    }
}
