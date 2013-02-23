using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TelChina.AF.Demo.DemoSV.Interfaces
{
    [ServiceContract]
    public interface ICommonCRUDService
    {
        //[OperationContract]
        void Add(List<object> entityList);

        [OperationContract]
        void Save(List<object> entityList);
    }
}
