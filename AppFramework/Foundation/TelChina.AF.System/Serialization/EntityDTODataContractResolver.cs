using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Reflection;
using TelChina.AF.Util.Logging;
using TelChina.AF.Persistant;

namespace TelChina.AF.Sys.Serialization
{
    /// <summary>
    /// 实体,DTO做为通用CRUD服务参数时,确定序列化和反序列化的标识
    /// </summary>
    public class EntityDTODataContractResolver : DataContractResolver
    {
        internal class TypeInfo
        {
            public XmlDictionaryString TypeName;
            public XmlDictionaryString AssemblyName;
        }


        private static readonly Dictionary<string, Assembly> AssemblyDic
            = new Dictionary<string, Assembly>();
        private static readonly Dictionary<Type, TypeInfo> TypeDic
           = new Dictionary<Type, TypeInfo>();

        private ILogger _logger = LogManager.GetLogger("DataContractResolver");
        //public EntityDTODataContractResolver()
        //{

        //}
        //private Assembly assembly;
        //public EntityDTODataContractResolver(Assembly assembly)
        //{
        //    this.assembly = assembly;
        //}


        /// <summary>
        /// 反序列化时通过解析类型名称得到类型
        /// Allows users to map xsi:type name to any Type 
        /// </summary>
        /// <param name="typeFullName">实体/DTO全名</param>
        /// <param name="assemblyFullName">实体/DTO所在的Assembly名称</param>
        /// <param name="declaredType">契约上定义的类型</param>
        /// <param name="knownTypeResolver">已知类型处理器</param>
        /// <returns></returns>
        public override Type ResolveName(string typeFullName, string assemblyFullName, Type declaredType,
            DataContractResolver knownTypeResolver)
        {

            if (declaredType.FullName == typeFullName)
                return declaredType;
            else
            {
                if (!AssemblyDic.ContainsKey(assemblyFullName))
                {
                    var type = knownTypeResolver.ResolveName(typeFullName, assemblyFullName, declaredType, knownTypeResolver);
                    if (type == null)
                    {
                        AssemblyDic.Add(assemblyFullName, Assembly.Load(assemblyFullName));
                    }
                    else
                    {
                        return type;
                    }
                }
                return AssemblyDic[assemblyFullName].GetType(typeFullName);
            }
        }

        // Used at serialization
        // Maps any Type to a new xsi:type representation
        /// <summary>
        /// 序列化时将类确定传输用的类型名称
        /// </summary>
        /// <param name="type">需要处理的类型,默认情况下应该传入的是实体或者DTO类型,通用CRUD查询实体时传入的是实体的代理子类对象,
        /// 所以需要特别处理</param>
        /// <param name="declaredType">契约上定义的类型</param>
        /// <param name="knownTypeResolver">已知类型处理器</param>
        /// <param name="typeName">类型全名</param>
        /// <param name="typeNamespace">类型所在Assembly</param>
        /// <returns></returns>
        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver,
            out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            typeName = new XmlDictionaryString(XmlDictionary.Empty, "", 0);
            typeNamespace = new XmlDictionaryString(XmlDictionary.Empty, "", 0);


            //if (dataContract == null)
            //{
            //    return false;
            //}
            //if (type == null)
            //{
            //    return false;s
            //}

            try
            {
                //优先使用系统定义的策略
                if (knownTypeResolver.TryResolveType(type, declaredType, knownTypeResolver, out typeName, out typeNamespace))
                {
                    return true;
                }

                //判断是否代理子类对象
                if (type.BaseType != null && type.Assembly.IsDynamic)
                {
                    //使用基类来代替
                    type = type.BaseType;
                }

                if (!TypeDic.ContainsKey(type))
                {
                    GetTypeInfo(type);
                }
                typeName = TypeDic[type].TypeName;
                typeNamespace = TypeDic[type].AssemblyName;

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        private static void GetTypeInfo(Type type)
        {
            var resolver = type.GetCustomAttributes(typeof(DataContractResolverAttribute), false)
                               .FirstOrDefault() as DataContractResolverAttribute;
            string tName;
            string aName;
            if (resolver != null)
            {
                tName = resolver.TypeFullName;
                aName = resolver.TypeAssembly;
            }
            else
            {
                tName = type.FullName;
                aName = type.Assembly.FullName;
            }
            var typeName = CreateXmlDicString(tName);
            var typeNamespace = CreateXmlDicString(aName);

            TypeDic.Add(type, new TypeInfo() { TypeName = typeName, AssemblyName = typeNamespace });

        }

        private static XmlDictionaryString CreateXmlDicString(string name)
        {
            var typeName = new XmlDictionaryString(XmlDictionary.Empty, name, 0);
            return typeName;
        }
    }
}
