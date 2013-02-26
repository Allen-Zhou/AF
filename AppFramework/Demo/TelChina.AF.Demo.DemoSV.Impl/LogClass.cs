using System;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.DemoSV
{
    public partial class LogClass2:EntityBase
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
            LogClass2 logClass = new LogClass2();
            logClass.Operating = opertion;
            logClass.ByteImage = asa;
            repo.Add(logClass);
            repo.SaveChanges();
        }
    }
}
