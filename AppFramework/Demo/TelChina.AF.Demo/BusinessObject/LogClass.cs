using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class LogClass:EntityBase
    {
        

        protected override void OnDeleted()
        {
            
        }

        protected override void OnDeleting()
        {
            
        }

        protected override void OnInserted()
        {
           
        }

        protected override void OnInserting()
        {
            
        }

        protected override void OnUpdated()
        {
           
        }

        protected override void OnUpdating()
        {
            
        }

        protected override void OnValidate()
        {
           
        }

        protected override void SetDefaultValue()
        {
            
        }

        public virtual void SaveLog(string opertion)
        {
         Byte[]  asa = new Byte[1];
         asa[0] = Byte.Parse("123");
            var repo = RepositoryContext.GetRepository();
            LogClass logClass = new LogClass();
            logClass.Operating = opertion;
            logClass.ByteImage = asa;
            repo.Add(logClass);
            repo.SaveChanges();
        }
    }
}
