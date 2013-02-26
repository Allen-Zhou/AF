using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TelChina.AF.Service.AOP;
using TelChina.AF.Sys.DTO;

namespace TelChina.AF.Resource
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ServiceContract]
    [PolicyInjectionBehavior] //提供PIAB接口
    public interface IEditResourceService
    {
        [OperationContract]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        IList<UserResourceDTO> GetTypeResource();

        [OperationContract]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        IList<UserResourceDTO> GetResourceByType(string resourceType);

        [OperationContract]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        void UpdateResource(IList<UserResourceDTO> userResources);
    }
}
