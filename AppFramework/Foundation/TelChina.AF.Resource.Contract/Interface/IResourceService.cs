using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Service.AOP;
using System.ServiceModel;
using System.Linq.Expressions;
using TelChina.AF.Sys.DTO;
using TelChina.AF.Sys.Service;

namespace TelChina.AF.Resource
{



    /// <summary>
    /// 服务接口
    /// </summary>
    [ServiceContract]
    [PolicyInjectionBehavior] //提供PIAB接口
    public interface IResourceService
    {
        [OperationContract(Name = "GetResource")]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        string GetResource(string resourceType, string resourceCode);

        [OperationContract(Name = "GetResourcesByResourceType")]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        Dictionary<string, string> GetResourcesByResourceType(string resourceType);

        [OperationContract(Name = "GetTypeResource")]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        string GetTypeResource(string resourceType);

        //[OperationContract(Name = "whereExpression")]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        string GetResource<TEntity>(Expression<Func<TEntity, object>> whereExpression) where TEntity : DTOBase;

        [OperationContract(Name = "DTOBase")]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        Dictionary<string, string> GetResourcesByResourceType(DTOBase dtoBase);

        [OperationContract(Name = "GetTypeDTOBase")]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        string GetTypeResource(DTOBase dtoBase);
    }
}
