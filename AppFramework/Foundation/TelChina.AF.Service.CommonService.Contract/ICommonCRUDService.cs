using System.ServiceModel;
using System;
using System.Collections.Generic;
namespace TelChina.AF.Service.CommonService.Contract
{
    [ServiceContract]
    public interface ICommonCRUDService
    {
        [OperationContract]
        void SaveChanges(List<object> entity);
        //[OperationContract]
        //List<Object> GetEntity();
        //[OperationContract]
        //object GetByID(string entityType, Guid entityID);
    }
}
