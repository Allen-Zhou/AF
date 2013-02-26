using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace TelChina.AF.Tool.Checked
{
    public interface IModelingEvent
    {
        /// <summary>
        /// Store 注册事件
        /// </summary>
        /// <param name="store"></param>
        void Register(Microsoft.VisualStudio.Modeling.Store store);

        void ValidateNames(UMLClassType umlType, ValidationContext context, object model);
    }
}
