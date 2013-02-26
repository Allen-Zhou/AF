using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Uml.Classes;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Uml.AuxiliaryConstructs;
using System.Drawing;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.Uml.Profiles;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace TelChina.AF.Tool.Checked
{
    public class CheckClassModel
    {

        #region 效验
        [Export(typeof(System.Action<ValidationContext, object>))]
        [ValidationMethod(
           ValidationCategories.Save
         | ValidationCategories.Menu)]
        public void ValidateClassNames(ValidationContext context, IClass elementToValidate)// This type determines what elements // will be validated by this method:
        {
            IModelingEvent modelingEvent = ValidateFactory.GetIModelingEvent();

            if (modelingEvent != null)
            {
                modelingEvent.ValidateNames(UMLClassType.Class, context, elementToValidate);
            }
        }

        [Export(typeof(System.Action<ValidationContext, object>))]
        [ValidationMethod(
           ValidationCategories.Save
         | ValidationCategories.Menu)]
        public void ValidateTypeName(ValidationContext context, Microsoft.VisualStudio.Uml.Classes.IProperty property)
        {
            IModelingEvent modelingEvent = ValidateFactory.GetIModelingEvent();

            if (modelingEvent != null)
            {
                modelingEvent.ValidateNames(UMLClassType.Property, context, property);
            }
        }



        [Export(typeof(System.Action<ValidationContext, object>))]
        [ValidationMethod(
           ValidationCategories.Save
         | ValidationCategories.Menu)]
        public void ValidateTypeName(ValidationContext context, Microsoft.VisualStudio.Uml.Classes.IInterface elementToValidate)
        {
            IModelingEvent modelingEvent = ValidateFactory.GetIModelingEvent();

            if (modelingEvent != null)
            {
                modelingEvent.ValidateNames(UMLClassType.Interface, context, elementToValidate);
            }
        }

        [Export(typeof(System.Action<ValidationContext, object>))]
        [ValidationMethod(
           ValidationCategories.Save
         | ValidationCategories.Menu)]
        public void ValidateTypeName(ValidationContext context, IPackage pachage)
        {
            IModelingEvent modelingEvent = ValidateFactory.GetIModelingEvent();

            if (modelingEvent != null)
            {
                modelingEvent.ValidateNames(UMLClassType.Package, context, pachage);
            }
        }

        #endregion 效验

        /// <summary>
        /// This isn't actually validation. It's to couple this adapter to the model before we start.
        /// The validation package creates an instance of this class and then calls this method.
        /// See "Validation": http://msdn.microsoft.com/library/bb126413.aspx
        /// </summary>
        /// <param name="vcontext"></param>
        /// <param name="model"></param>
        [Export(typeof(System.Action<ValidationContext, object>))]
        [ValidationMethod(ValidationCategories.Open)]
        public void ConnectAdapterToModel(ValidationContext vcontext, IModel model)
        {
            Store store = (model as ModelElement).Store;

            IModelingEvent modelingEvent = ValidateFactory.GetIModelingEvent();

            if (modelingEvent != null)
            {
                modelingEvent.Register(store);
            }
        }

    }

}