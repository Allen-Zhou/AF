using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using TelChina.AF.Persistant;
//using TelChina.AF.Demo.DomainModel.Model;
//using TelChina.AF.Domain.Core;

namespace TelChina.AF.Demo.DemoSV
{

    partial class TransSV
    {
        private Guid DoExtend(bool isSucceed)
        {
            IRepository repository = RepositoryContext.GetRepository();
            Category entity = this.CreateEntity();
            repository.Add(entity);
            repository.SaveChanges();
            //bool actual = repository.SaveChanges() > 0;
            //if (!actual)
            //{
            //    throw new TelChina.AF.Demo.DemoSV.Exceptions.InvalidOperationException("实体新增失败");

            //    //if (false)
            //    //    Console.WriteLine();
            //}
            if (!isSucceed)
            {
                throw new TelChina.AF.Demo.DemoSV.Exceptions.ExpectedException();
            }
            return entity.ID;
        }

        private Category CreateEntity()
        {
            var category = new Category(); // TODO: Initialize to an appropriate value
            category.Name = "Name" + EntityCount;
            category.Description = "ParamDsc" + EntityCount;
            category.Size = EntityCount;
            EntityCount++;
            return category;
        }

        private int EntityCount { get; set; }
    }
}
