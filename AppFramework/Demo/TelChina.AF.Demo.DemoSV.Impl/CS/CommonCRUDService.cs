using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Demo.DemoSV.Interfaces;
using TelChina.AF.Persistant;
using TelChina.AF.Util.Logging;
using TelChina.AF.Sys.Service;

namespace TelChina.AF.Demo.DemoSV
{
    public class CommonCRUDService : ServiceBase, ICommonCRUDService
    {
        private readonly ILogger _logger =
            LogManager.GetLogger(typeof(CommonCRUDService).FullName);

        public void Add(List<object> entityList)
        {
            base.ServiceInvoke(() => Add_Extend(entityList));
        }

        public void Save(List<object> entityList)
        {
            base.ServiceInvoke(() => Save_Extend(entityList));
        }

        public object GetByKey(EntityKey entityKey)
        {
            return base.ServiceInvoke(() => GetByKey_Extend(entityKey));
        }

        public object GetByKey_Extend(EntityKey entityKey)
        {
            if (entityKey == null || entityKey.IsEmpty)
                return null;

            using (var repo = RepositoryContext.GetRepository())
            {
                var result = repo.GetByKey(entityKey);
                return result;
            }
        }

        private void Save_Extend(List<object> entityList)
        {
            if (entityList != null && entityList.Count > 0)
            {
                var repo = RepositoryContext.GetRepository();
                foreach (EntityBase entity in entityList)
                {
                    switch (entity.SysState)
                    {
                        case EntityStateEnum.Inserting:
                            {
                                repo.Add(entity);
                                break;
                            }
                        case EntityStateEnum.Updating:
                            {
                                repo.Update(entity);
                                break;
                            }
                        case EntityStateEnum.Deleting:
                            {
                                repo.Remove(entity);
                                break;
                            }
                        case EntityStateEnum.Unchanged:
                        default:
                            {
                                _logger.Info(string.Format("提交的实体{0}状态为Unchanged", entity.GetType()));
                                break;
                            }
                    }
                }
                repo.SaveChanges();
            }
        }

        private void Add_Extend(List<object> entityList)
        {
            //if (entityList != null && entityList.Count > 0)
            //{
            //    foreach (var entity in entityList)
            //    {
            //        _logger.Debug(string.Format("参数类型:{0},参数值:{1}",
            //                                    entity.GetType().FullName, entity.ToString()));
            //    }
            //}
        }
    }
}
