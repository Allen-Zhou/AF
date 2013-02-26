using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.DemoSV.Interfaces
{
    [ServiceContract]
    public interface ICommonCRUDService
    {
        //[OperationContract]
        void Add(List<object> entityList);

        [OperationContract]
        void Save(List<object> entityList);

        [OperationContract]
        object GetByKey(EntityKey entityKey);
    }
}
