using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.DemoSV
{

    internal partial class DemoSVImplement
    {
        public ResultDTO Do_Ex()
        {
            Console.WriteLine(string.Format("服务DemoSV正在执行...参数:{0}", Param.ToString()));
            using (var repo = RepositoryContext.GetRepository())
            {
                var query = (from d in repo.Query<Department>()
                             where d.Code == "xxx"
                             
                             select d).ToList();

            }

            if (Param.IsSucceed)
            {
                ResultDTO result = new ResultDTO();
                result.ReturnValue = Param.ParamName + "," + Param.Value;
                return result;
            }
            else
            {
                throw new DemoSVExecption("服务调用异常!,测试用");
            }
        }
    }
}
