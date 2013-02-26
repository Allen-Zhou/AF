using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using TelChina.AF.Tool.Checked;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Uml.Profiles;


namespace TelChina.AF.Tool.CheckImpl
{
    public class ModelExtendEvent : IModelingEvent
    {
        private Store modelingStore;

        public void Register(Microsoft.VisualStudio.Modeling.Store store)
        {
            modelingStore = store;
            var siClass = store.DomainDataDirectory.FindDomainClass
      ("Microsoft.VisualStudio.Uml.Classes.Property");
            store.EventManagerDirectory.ElementAdded.Add(siClass,
              new EventHandler<ElementAddedEventArgs>(PropertyStereotypeAdded));
            //Operation
            var siopertion = store.DomainDataDirectory.FindDomainClass
          ("Microsoft.VisualStudio.Uml.Classes.Operation");
            store.EventManagerDirectory.ElementAdded.Add(siopertion,
              new EventHandler<ElementAddedEventArgs>(OpertionStereotypeAdded));


            //删除Class
            var siDeleteClass = store.DomainDataDirectory.FindDomainClass
         ("Microsoft.VisualStudio.Uml.Classes.Class");
            store.EventManagerDirectory.ElementDeleted.Add(siDeleteClass,
              new EventHandler<ElementDeletedEventArgs>(DeleteClass));

            //删除Interface
            var siDeleteInterface = store.DomainDataDirectory.FindDomainClass
         ("Microsoft.VisualStudio.Uml.Classes.Interface");
            store.EventManagerDirectory.ElementDeleted.Add(siDeleteInterface,
              new EventHandler<ElementDeletedEventArgs>(DeleteInterface));

            //删除Package
            var siDeletePackage = store.DomainDataDirectory.FindDomainClass
         ("Microsoft.VisualStudio.Uml.Classes.Package");
            store.EventManagerDirectory.ElementDeleted.Add(siDeletePackage,
              new EventHandler<ElementDeletedEventArgs>(DeletePackage));
            //属性改变 Class           
            store.EventManagerDirectory.ElementPropertyChanged.Add(siDeleteClass,
              new EventHandler<ElementPropertyChangedEventArgs>(PropertyChangedClass));
            //属性改变 Interface           
            store.EventManagerDirectory.ElementPropertyChanged.Add(siDeleteInterface,
              new EventHandler<ElementPropertyChangedEventArgs>(PropertyChangedInterface));

            //属性改变 Package           
            store.EventManagerDirectory.ElementPropertyChanged.Add(siDeletePackage,
              new EventHandler<ElementPropertyChangedEventArgs>(PropertyChangedPackage));
        }
        
        public void ValidateNames(UMLClassType umlType, ValidationContext context, object model)
        {
            switch (umlType)
            {
                case UMLClassType.Class:
                    IClass classModel = model as IClass;
                    ValidateClassName(context, classModel);
                    break;
                case UMLClassType.Property:
                    Microsoft.VisualStudio.Uml.Classes.IProperty property = model as Microsoft.VisualStudio.Uml.Classes.IProperty;
                    ValidatePropertyName(context, property);
                    break;
                case UMLClassType.Interface:
                    IInterface interfaceModel = model as IInterface;
                    ValidateInterfaceName(context, interfaceModel);
                    break;
                case UMLClassType.Package:
                    IPackage packageModel = model as IPackage;
                    ValidatePackageName(context, packageModel);
                    break;
                default:
                    break;
            }
        }

        #region 类验证
        /// <summary>
        /// 类验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="classModel"></param>
        public void ValidateClassName(ValidationContext context, IClass classModel)
        {
            List<string> attributeNames = new List<string>();
            int CountOperation = classModel.OwnedOperations.Count();
            if (CountOperation > 0)
            {
                context.LogWarning(
                      string.Format("class {0}中存在{1}个方法", classModel.Name, CountOperation),
                      "001", classModel);
            }
            if (classModel.OwningPackage == null)
            {
                context.LogError(
                     string.Format("class {0}不是在包中", classModel.Name),
                     "001", classModel);
            }
            foreach (Microsoft.VisualStudio.Uml.Classes.IProperty attribute in classModel.OwnedAttributes)
            {
                string name = attribute.Name;
                if (!string.IsNullOrEmpty(name) && attributeNames.Contains(name.ToLower()))
                {
                    context.LogError(
                      string.Format("类 '{0}'中已经存在属性{1}", classModel.Name, name),
                      "002", classModel);
                }
                attributeNames.Add(name.ToLower());
            }
            bool isExistModel = false;
            var packages = classModel.GetModelStore().Root.NestedPackages;
            foreach (IPackage package in packages)
            {
                isExistModel = isExistSameName(package, classModel);
                if (isExistModel == true)
                {
                    break;
                }
            }
            if (isExistModel)
            {
                context.LogError(
                      string.Format("包{0}中的类{1}名称已经被使用，请修改！", classModel.OwningPackage.Name, classModel.Name),
                      "002", classModel);
            }
        }

        /// <summary>
        /// 模型上是否存在此模型接口
        /// </summary>
        /// <param name="package">包</param>
        /// <param name="classModel">模型</param>
        /// <returns></returns>
        private bool isExistSameName(IPackage package, IClass classModel)
        {
            //模型名称
            string modelName = classModel.Name;
            bool isExist = false;
            var elements = package.OwnedElements;
            foreach (IElement owenElement in elements)
            {
                if (owenElement is IInterface)
                {
                    if ((owenElement as IInterface).Name == modelName)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (owenElement is IClass)
                {
                    if ((owenElement as IClass).Name == modelName && (owenElement as IClass) != classModel)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (owenElement is IPackage)
                {
                    isExist = isExistSameName(owenElement as IPackage, classModel);
                    if (isExist == true)
                    {
                        break;
                    }
                }
            }
            return isExist;
        }

        #endregion

        #region 特性验证
        /// <summary>
        /// 特性验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="property"></param>
        public void ValidatePropertyName(ValidationContext context, Microsoft.VisualStudio.Uml.Classes.IProperty property)
        {
            if ((property.Type == null || property.Type.Name.Trim().Length < 1) && property.Classifier is IClass)
            {
                context.LogError("类" + property.Class.Name + "中的属性" + property.Name + "的Type不能为空，请选择！", "001", property);
            }

            string length = GetStereotypePropertyValue(property as IElement, "EntityBaseProperty", "Length");
            string precision = GetStereotypePropertyValue(property as IElement, "EntityBaseProperty", "Precision");
            if (!string.IsNullOrEmpty(length))
            {
                int outLength;
                bool isInt = int.TryParse(length, out outLength);

                if (!isInt)
                {
                    context.LogError("类" + property.Class.Name + "中的属性" + property.Name + "的长度不是整数类型，请修改！", "015", property);
                }
            }
            if (!string.IsNullOrEmpty(precision))
            {
                int outPrecision;
                bool isInt = int.TryParse(precision, out outPrecision);

                if (!isInt)
                {
                    context.LogError("类" + property.Class.Name + "中的属性" + property.Name + "的小数位数不是整数类型，请修改！", "020", property);
                }
            }
        }
        #endregion

        #region 接口验证
        /// <summary>
        /// 接口验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="interfaceModel"></param>
        public void ValidateInterfaceName(ValidationContext context, IInterface interfaceModel)
        {
            if (interfaceModel.OwnedAttributes.Count() > 0)
            {
                context.LogWarning(
                     string.Format("Interface {0}中存在属性", interfaceModel.Name),
                     "001", interfaceModel);
            }
        }
        #endregion

        #region 包验证
        /// <summary>
        /// 包验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="packageModel"></param>
        public void ValidatePackageName(ValidationContext context, IPackage packageModel)
        {
            bool isExistModel = false;
            var packages = packageModel.GetModelStore().Root.NestedPackages;
            foreach (IPackage package in packages)
            {
                isExistModel = isExistSamePackageName(package, packageModel);
                if (isExistModel == true)
                {
                    break;
                }
            }
            if (isExistModel)
            {
                context.LogError(
                      string.Format("包{0}的名称已经被其他包使用，请修改！", packageModel.Name),
                      "002", packageModel);
            }
        }

        /// <summary>
        /// 模型上是否存在此模型接口
        /// </summary>
        /// <param name="package">模型中的所有包</param>
        /// <param name="packageModel">需要检查的包模型</param>
        /// <returns></returns>
        private bool isExistSamePackageName(IPackage package, IPackage packageModel)
        {
            //模型名称
            string modelName = packageModel.Name;
            bool isExist = false;
            if (package == null)
            {
                return isExist;
            }
            var elements = package.NestedPackages;
            foreach (IPackage owenElement in elements)
            {
                if (owenElement.Name == modelName && owenElement != packageModel)
                {
                    isExist = true;
                    break;
                }
                else
                {
                    isExist = isExistSamePackageName(owenElement, packageModel);
                    if (isExist == true)
                    {
                        break;
                    }
                }
            }
            return isExist;
        }
        #endregion


        #region 应用扩展项目
        /// <summary>
        /// Event handler called whenever a stereotype instance is linked to a uml model element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyStereotypeAdded(object sender, ElementAddedEventArgs e)
        {
            // Don't handle changes in undo or load from file:
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;

            IElement property = e.ModelElement as IElement;

            ApplyStereotype(property, "TRFEntityBaseModelProfile", "EntityBaseProperty");
        }

        /// <summary>
        /// 设置操作上的扩展属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpertionStereotypeAdded(object sender, ElementAddedEventArgs e)
        {
            // Don't handle changes in undo or load from file:
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;

            IElement opertion = e.ModelElement as IElement;

            ApplyStereotype(opertion, "TRFEntityBaseModelProfile", "TelChinaOperation");
        }


        /// <summary>
        /// 应用扩展项目
        /// </summary>
        /// <param name="opertion">模型对象</param>
        /// <param name="ProfileName"></param>
        /// <param name="StereotypeName"></param>
        private void ApplyStereotype(IElement opertion, string ProfileName, string StereotypeName)
        {
            var q = opertion.GetModelStore().ProfileManager.AllProfiles.ToList();
            foreach (IProfile ip in q)
            {
                if (ip.Name == ProfileName)
                {
                    foreach (IStereotype istype in ip.Stereotypes)
                    {
                        if (istype.Name == StereotypeName && opertion.ApplicableStereotypes.ToList().Contains(istype))
                        {
                            var stereotypeInstaces = opertion.AppliedStereotypes.ToList();

                            int count = 0;
                            foreach (IStereotypeInstance isInstance in stereotypeInstaces)
                            {
                                if (isInstance.Name == istype.Name)
                                {
                                    count = count + 1;
                                    break;
                                }
                            }

                            if (count == 0)
                            {
                                opertion.ApplyStereotype(istype);
                            }
                        }
                    }
                }
            }
        }

        #endregion 应用扩展项目

        #region 模型修改名称和删除

        private ArrayList classTemplateList = new ArrayList() { "TelChinaClassTemplate", "TelChinaMethodTemplate", "TelChinaMappingTemplate", "TelChinaSQLTemplate", "TelChinaORCTemplate", "TelChinaResourceSQLTemplate", "TelChinaResourceORCTemplate" };
        private ArrayList dtoTemplateList = new ArrayList() { "TelChinaClassToDTOTemplate" };
        private ArrayList interfaceTemplateList = new ArrayList() { "TelChinaInterfaceTemplate" };

        private ArrayList contractTemplateList = new ArrayList() { "TelChinaImplAutoTemplate", "TelChinaImplTemplate", "TelChinaSVCTemplate" };

        #region 模型删除

        #region 删除类
        /// <summary>
        /// 删除类 从而删除自动生成的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClass(object sender, ElementDeletedEventArgs e)
        {
            // Don't handle changes in undo or load from file:
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;

            IElement element = e.ModelElement as IElement;

            string modelName = ModelNameByType(element, 0);
            bool isExist = isExistModel(element, 0, modelName);

            if (isExist)
            {
                return;
            }
            RemoveProjectItemByType(element, 0, modelName);
        }

        /// <summary>
        /// 模型上是否存在此模型类
        /// </summary>
        /// <param name="element">模型对象</param>
        /// <param name="modelName">模型名称</param>
        /// <returns></returns>
        private bool isExistClassModel(IPackage package, string modelName)
        {
            bool isExist = false;
            var elements = package.OwnedElements;
            foreach (IElement owenElement in elements)
            {
                if (owenElement is IClass)
                {
                    if ((owenElement as IClass).Name == modelName)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (owenElement is IPackage)
                {
                    isExist = isExistClassModel(owenElement as IPackage, modelName);
                    if (isExist == true)
                    {
                        break;
                    }
                }
            }
            return isExist;
        }

        #endregion

        #region  删除接口
        /// <summary>
        /// 删除接口 从而删除自动生成的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteInterface(object sender, ElementDeletedEventArgs e)
        {
            // Don't handle changes in undo or load from file:
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;

            IElement element = e.ModelElement as IElement;
            string modelName = ModelNameByType(element, 1);

            bool isExist = isExistModel(element, 1, modelName);

            if (isExist)
            {
                return;
            }

            RemoveProjectItemByType(element, 1, modelName);

        }

        /// <summary>
        /// 模型上是否存在此模型接口
        /// </summary>
        /// <param name="element">模型对象</param>
        /// <param name="modelName">模型名称</param>
        /// <returns></returns>
        private bool isExistInterfaceModel(IPackage package, string modelName)
        {
            bool isExist = false;
            var elements = package.OwnedElements;
            foreach (IElement owenElement in elements)
            {
                if (owenElement is IInterface)
                {
                    if ((owenElement as IInterface).Name == modelName)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (owenElement is IPackage)
                {
                    isExist = isExistInterfaceModel(owenElement as IPackage, modelName);
                    if (isExist == true)
                    {
                        break;
                    }
                }
            }
            return isExist;
        }

        #endregion

        #region  删除包
        /// <summary>
        /// 删除包 从而删除自动生成的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePackage(object sender, ElementDeletedEventArgs e)
        {
            // Don't handle changes in undo or load from file:
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;

            IElement element = e.ModelElement as IElement;
            string modelName = ModelNameByType(element, 2);

            bool isExist = isExistModel(element, 2, modelName);

            if (isExist)
            {
                return;
            }

            RemoveProjectItemByType(element, 2, modelName);

        }

        /// <summary>
        /// 模型上是否存在此模型接口
        /// </summary>
        /// <param name="element">模型对象</param>
        /// <param name="modelName">模型名称</param>
        /// <returns></returns>
        private bool isExistPackageModel(IPackage package, string modelName)
        {
            bool isExist = false;
            if (package == null)
            {
                return isExist;
            }
            var elements = package.NestedPackages;
            foreach (IPackage owenElement in elements)
            {
                if (owenElement.Name == modelName)
                {
                    isExist = true;
                    break;
                }
                else
                {
                    isExist = isExistPackageModel(owenElement, modelName);
                    if (isExist == true)
                    {
                        break;
                    }
                }
            }
            return isExist;
        }
        #endregion

        #endregion 模型删除

        #region  名称改变

        #region 类名称改变
        /// <summary>
        /// 类属性名称改变删除生成的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyChangedClass(object sender, ElementPropertyChangedEventArgs e)
        {
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;
            //不是修改的类名称
            if (e.DomainProperty.Name != "Name")
                return;
            IElement element = e.ModelElement as IElement;

            string modelName = e.OldValue.ToString();

            RemoveProjectItemByType(element, 0, modelName);
        }
        #endregion 类名称改变

        #region 接口名称改变
        /// <summary>
        /// 类属性名称改变删除生成的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyChangedInterface(object sender, ElementPropertyChangedEventArgs e)
        {
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;
            //不是修改的类名称
            if (e.DomainProperty.Name != "Name")
                return;
            IElement element = e.ModelElement as IElement;

            string modelName = e.OldValue.ToString();

            RemoveProjectItemByType(element, 1, modelName);

        }
        #endregion 接口名称改变

        #region 包名称改变
        /// <summary>
        /// 类属性名称改变删除生成的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyChangedPackage(object sender, ElementPropertyChangedEventArgs e)
        {
            if (modelingStore.InUndoRedoOrRollback || modelingStore.InSerializationTransaction) return;
            //不是修改的类名称
            if (e.DomainProperty.Name != "Name")
                return;

            IElement element = e.ModelElement as IElement;

            string modelName = e.OldValue.ToString();

            RemoveProjectItemByType(element, 2, modelName);
        }
        #endregion 包名称改变

        #endregion

        /// <summary>
        /// 得到模型名称
        /// </summary>
        /// <param name="element">模型对象</param>
        /// <param name="modeType">模型类型</param>
        /// <returns>模型名称</returns>
        private string ModelNameByType(IElement element, int modeType)
        {
            string modelName = string.Empty;
            if (modeType == 0)
            {
                modelName = (element as IClass).Name;
            }
            if (modeType == 1)
            {
                modelName = (element as IInterface).Name;
            }
            if (modeType == 2)
            {
                modelName = (element as IPackage).Name;
            }

            return modelName;
        }

        /// <summary>
        /// 模型是否存在
        /// </summary>
        /// <param name="element">模型对象</param>
        /// <param name="modeType">模型类型0==类  1==接口  2==包</param>
        /// <param name="modelName">模型名称</param>
        /// <returns>模型是否存在</returns>
        private bool isExistModel(IElement element, int modeType, string modelName)
        {
            bool isExistModel = false;
            var packages = element.GetModelStore().Root.NestedPackages;
            foreach (IPackage package in packages)
            {
                isExistModel = isExistModelInPackage(package, modeType, modelName);
                if (isExistModel == true)
                {
                    break;
                }
            }
            return isExistModel;
        }
        /// <summary>
        /// 模型是否存在包中
        /// </summary>
        /// <param name="package">包对象</param>
        /// <param name="modeType">模型类型0==类  1==接口  2==包</param>
        /// <param name="modelName">模型名称</param>
        /// <returns>模型是否存在包中</returns>
        private bool isExistModelInPackage(IPackage package, int modeType, string modelName)
        {
            bool isExist = false;
            if (modeType == 0)
            {
                isExist = isExistClassModel(package, modelName);
            }
            if (modeType == 1)
            {
                isExist = isExistInterfaceModel(package, modelName);
            }
            if (modeType == 2)
            {
                isExist = isExistPackageModel(package, modelName);
            }

            return isExist;
        }

        /// <summary>
        /// 通过模型类型移除项目
        /// </summary>
        /// <param name="element">模型对象</param>
        /// <param name="modeType">模型类型0==类  1==接口  2==包</param>
        /// <param name="modelName">模型名称</param>
        private void RemoveProjectItemByType(IElement element, int modeType, string modelName)
        {
            EnvDTE80.DTE2 dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

            //获得TextTemplateBindings            
            //项目路径
            string projectsPath = GetStereotypePropertyValue(element.GetModelStore().Root, "TextTemplateBindings", "ProjectPath"); ;

            //文件名称
            string targetsNames = GetStereotypePropertyValue(element.GetModelStore().Root, "TextTemplateBindings", "TargetName");

            //类项目名称
            string classProjectName = GetTemplateProjectName(projectsPath, "TelChinaClassTemplate");

            //DTO项目名称
            string dtoProjectName = GetTemplateProjectName(projectsPath, "TelChinaClassToDTOTemplate");

            //接口项目名称
            string interfaceProjectName = GetTemplateProjectName(projectsPath, "TelChinaInterfaceTemplate");

            //实现项目名称
            string contractProjectName = GetTemplateProjectName(projectsPath, "TelChinaImplTemplate");
            //类文件集合
            List<string> classFileNameList = new List<string>();
            //DTO文件集合
            List<string> dtoFileNameList = new List<string>();
            //类文件集合
            List<string> interfaceFileNameList = new List<string>();
            //DTO文件集合
            List<string> contractFileNameList = new List<string>();
            if (modeType == 2)
            {
                classFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.Package);

                dtoFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.Package);

                interfaceFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.Package);

                contractFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.Package);
            }
            else
            {
                classFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.Class);

                dtoFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.DTO);

                interfaceFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.Interface);

                contractFileNameList = GetFileName(targetsNames, modelName, TemplateEnum.Contract);
            }
            //取得project
            var sln = dte.Solution;

            bool isExistFile = false;
            if (classFileNameList.Count > 0)
            {
                foreach (string fileName in classFileNameList)
                {
                    isExistFile = IsExistProjectItem(sln, classProjectName, fileName);
                    if (isExistFile)
                    { break; }
                }
            }
            if (dtoFileNameList.Count > 0 && !isExistFile)
            {
                foreach (string fileName in dtoFileNameList)
                {
                    isExistFile = IsExistProjectItem(sln, dtoProjectName, fileName);
                    if (isExistFile)
                    { break; }
                }
            }
            if (interfaceFileNameList.Count > 0 && !isExistFile)
            {
                foreach (string fileName in interfaceFileNameList)
                {
                    isExistFile = IsExistProjectItem(sln, interfaceProjectName, fileName);
                    if (isExistFile)
                    { break; }
                }
            }

            if (contractFileNameList.Count > 0 && !isExistFile)
            {
                foreach (string fileName in contractFileNameList)
                {
                    isExistFile = IsExistProjectItem(sln, contractProjectName, fileName);
                    if (isExistFile)
                    { break; }
                }
            }
            if (!isExistFile)
            {
                return;
            }
            //移除类
            if (classFileNameList.Count > 0)
            {
                foreach (string fileName in classFileNameList)
                {
                    RemoveItemFromSolution(sln, classProjectName, fileName);
                }
            }
            //移除DTO
            if (dtoFileNameList.Count > 0)
            {
                foreach (string fileName in dtoFileNameList)
                {
                    RemoveItemFromSolution(sln, dtoProjectName, fileName);
                }
            }
            //移除接口
            if (interfaceFileNameList.Count > 0)
            {
                foreach (string fileName in interfaceFileNameList)
                {
                    RemoveItemFromSolution(sln, interfaceProjectName, fileName);
                }
            }
            //移除实现
            if (contractFileNameList.Count > 0)
            {
                foreach (string fileName in contractFileNameList)
                {
                    RemoveItemFromSolution(sln, contractProjectName, fileName);
                }
            }
        }

        /// <summary>
        /// 返回模型上扩展项的值
        /// </summary>
        /// <param name="opertion"></param>
        /// <param name="stereotypeName"></param>
        /// <param name="stereotypePropertyName"></param>
        /// <returns></returns>
        private string GetStereotypePropertyValue(IElement element, string stereotypeName, string stereotypePropertyName)
        {
            string projectsPath = string.Empty;
            var q = element.AppliedStereotypes.ToList();
            foreach (IStereotypeInstance stereotypeInstance in q)
            {
                if (stereotypeInstance.Name == stereotypeName)
                {
                    foreach (IStereotypePropertyInstance stProperty in stereotypeInstance.PropertyInstances)
                    {
                        if (stProperty.Name == stereotypePropertyName)
                        {
                            projectsPath = stProperty.Value;
                        }
                    }
                }
            }
            return projectsPath;
        }

        /// <summary>
        /// 返回项目名称
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private string GetTemplateProjectName(string projectPath, string template)
        {
            string projectName = string.Empty;
            var paths = projectPath.Split('|');
            foreach (string path in paths)
            {
                if (path.Contains(template))
                {
                    var projectpaths = path.Split(new char[1] { '\\' });
                    foreach (string projectpath in projectpaths)
                    {
                        if (projectpath.Contains(".csproj"))
                        {
                            projectName = projectpath.Replace(".csproj", "");
                        }
                    }
                }
            }

            return projectName;
        }

        /// <summary>
        /// 得到生成文件的名称
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="className">类名称</param>
        /// <param name="templateEnum">哪种模版</param>
        private List<string> GetFileName(string targetsNames, string className, TemplateEnum templateEnum)
        {
            List<string> fileNameList = new List<string>();
            if (templateEnum == TemplateEnum.Package)
            {
                fileNameList.Add(className);
                return fileNameList;
            }
            var targetsName = targetsNames.Split('|');
            foreach (string targetName in targetsName)
            {
                //是否存在
                bool isExists = false;
                var fileNames = targetName.Split('=');
                if (templateEnum == TemplateEnum.Class && classTemplateList.Contains(fileNames[0]))
                {
                    isExists = true;
                }
                if (templateEnum == TemplateEnum.Contract && contractTemplateList.Contains(fileNames[0]))
                {
                    isExists = true;
                }
                if (templateEnum == TemplateEnum.DTO && dtoTemplateList.Contains(fileNames[0]))
                {
                    isExists = true;
                }
                if (templateEnum == TemplateEnum.Interface && interfaceTemplateList.Contains(fileNames[0]))
                {
                    isExists = true;
                }
                if (!isExists)
                {
                    continue;
                }
                var fileName = fileNames[1];
                fileName = fileName.Replace("{Name}", className);
                if (fileNames[0] == "TelChinaMappingTemplate")
                {
                    fileName += ".xml";
                }
                else if (fileNames[0] == "TelChinaSQLTemplate" || fileNames[0] == "TelChinaORCTemplate" || fileNames[0] == "TelChinaResourceSQLTemplate" || fileNames[0] == "TelChinaResourceORCTemplate")
                {
                    fileName += ".SQL";
                }
                else if (fileNames[0] == "TelChinaSVCTemplate")
                {
                    fileName += ".svc";
                }
                else
                {
                    fileName += ".cs";
                }
                fileNameList.Add(fileName);
            }

            return fileNameList;
        }

        /// <summary>
        /// 工程项目中的是否存在项
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="projectName">项目名称</param>
        /// <param name="itemName">项名称</param>
        private bool IsExistProjectItem(Solution solution, string projectName, string itemName)
        {
            bool isExist = false;
            foreach (Project project in solution.Projects)
            {
                if (project.Name == projectName)
                {
                    isExist = IsExistProjectItemInProject(project, itemName);
                }
                else
                {
                    if (project.ProjectItems.Count > 0)
                    {
                        foreach (ProjectItem projectItem in project.ProjectItems)
                        {
                            ProjectItem ItemProject = FindProject(projectItem, projectName);
                            if (ItemProject.Name == projectName)
                            {
                                isExist = IsExistProjectItemInProject(ItemProject.SubProject, itemName);
                                if (isExist)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (isExist)
                {
                    break;
                }
            }

            return isExist;
        }

        /// <summary>
        /// 工程项目中的是否存在项
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="itemName">项名称</param>
        private bool IsExistProjectItemInProject(Project project, string itemName)
        {
            bool isExist = false;
            foreach (ProjectItem projectItem in project.ProjectItems)
            {
                if (projectItem.Name == itemName)
                {
                    isExist = true;
                    break;
                }
                else
                {
                    isExist = IsExistProjectItemInItem(projectItem, itemName);
                    if (isExist)
                    {
                        break;
                    }
                }
            }
            return isExist;
        }

        /// <summary>
        /// 工程项目中的是否存在项
        /// </summary>
        /// <param name="projectItem">项目</param>
        /// <param name="itemName">项名称</param>
        private bool IsExistProjectItemInItem(ProjectItem projectItem, string itemName)
        {
            bool isExist = false;
            if (projectItem.ProjectItems == null || projectItem.ProjectItems.Count == 0)
            {
                return isExist;
            }
            ProjectItem item = FindProjectItem(projectItem, itemName);
            if (item.Name == itemName)
            {
                isExist = true;
            }
            return isExist;
        }

        /// <summary>
        /// 移除工程项目中的项
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="projectName">项目名称</param>
        /// <param name="itemName">项名称</param>
        /// /// <param name="isDelete">是否删除或者移除</param>
        private void RemoveItemFromSolution(Solution solution, string projectName, string itemName)
        {
            foreach (Project project in solution.Projects)
            {
                if (RemoveItemFromProject(project, projectName, itemName))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 移除工程项目中的项
        /// </summary>
        /// <param name="hierarchies"></param>
        /// <param name="projectName">项目名称</param>
        /// <param name="itemName">项名称</param>
        /// /// <param name="isDelete">是否删除或者移除</param>
        private bool RemoveItemFromProject(Project project, string projectName, string itemName)
        {
            bool isRemove = false;
            bool isDelete = false;
            if (itemName.Contains(".hbm.xml"))
            {
                isDelete = true;
            }
            if (project.Name == projectName)
            {
                foreach (ProjectItem projectItem in project.ProjectItems)
                {
                    isRemove = RemoveItemFromItems(projectItem, itemName, isDelete);
                    if (isRemove)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (project.ProjectItems.Count > 0)
                {
                    foreach (ProjectItem projectItem in project.ProjectItems)
                    {
                        ProjectItem ItemProject = FindProject(projectItem, projectName);
                        if (ItemProject.Name == projectName)
                        {
                            isRemove = RemoveItemFromProject(ItemProject.SubProject, projectName, itemName);
                            if (isRemove)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return isRemove;
        }

        /// <summary>
        /// 移除项目中的项
        /// </summary>
        /// <param name="projectItem"></param>
        /// <param name="itemName">项名称</param>
        /// <param name="isDelete">是否删除或者移除</param>
        private bool RemoveItemFromItems(ProjectItem projectItem, string itemName, bool isDelete)
        {
            bool isRemove = false;
            ProjectItem item;
            if (projectItem.Name == itemName)
            {
                item = projectItem;
                if (isDelete)
                {
                    item.Delete();
                }
                else
                {
                    item.Remove();
                }
                isRemove = true;
            }
            else if (projectItem.ProjectItems.Count > 0)
            {
                item = FindProjectItem(projectItem, itemName);
                if (item.Name == itemName)
                {
                    if (isDelete)
                    {
                        item.Delete();
                    }
                    else
                    {
                        item.Remove();
                    }
                    isRemove = true;
                }
            }

            return isRemove;
        }

        /// <summary>
        /// 根据项目名称得到项目
        /// </summary>
        /// <param name="project"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private ProjectItem FindProject(ProjectItem project, string projectName)
        {
            ProjectItem item = project;
            if (project.SubProject == null)
            {
                return item;
            }
            foreach (ProjectItem projectItem in project.SubProject.ProjectItems)
            {
                if (projectItem.Name == projectName)
                {
                    item = projectItem;
                    break;
                }
                else if (projectItem.SubProject != null && projectItem.SubProject.ProjectItems.Count > 0)
                {
                    item = FindProject(projectItem, projectName);
                    if (item.Name == projectName)
                    {
                        break;
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// 根据项目名称得到项目
        /// </summary>
        /// <param name="project"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private ProjectItem FindProjectItem(ProjectItem project, string itemName)
        {
            ProjectItem item = project;
            if (project.ProjectItems == null)
            {
                return item;
            }
            foreach (ProjectItem projectItem in project.ProjectItems)
            {
                if (projectItem.Name == itemName)
                {
                    item = projectItem;
                    break;
                }
                else if (projectItem.ProjectItems.Count > 0)
                {
                    item = FindProjectItem(projectItem, itemName);
                    if (item.Name == itemName)
                    {
                        break;
                    }
                }
            }

            return item;
        }


        #endregion 模型修改名称和删除
    }



    public enum TemplateEnum
    {
        /// <summary>
        /// 类
        /// </summary>
        Class,

        /// <summary>
        /// DTO
        /// </summary>
        DTO,

        /// <summary>
        /// 接口
        /// </summary>
        Interface,

        /// <summary>
        /// 实现
        /// </summary>
        Contract,

        /// <summary>
        /// 包
        /// </summary>
        Package

    }
}
