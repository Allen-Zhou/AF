using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public abstract  class Parent : EntityBase
    {
        //public new Guid ID
        //{
        //    get;
        //    set;
        //}

        public virtual string FirstName
        {
            get;
            set;
        }

        public virtual string LastName
        {
            get;
            set;
        }

        public override string EntityComponent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override void SetDefaultValue()
        {
            
        }

        protected override void OnValidate()
        {
            
        }

        protected override void OnInserting()
        {
            
        }

        protected override void OnInserted()
        {
        }

        protected override void OnUpdating()
        {
        }

        protected override void OnUpdated()
        {
        }

        protected override void OnDeleting()
        {
        }

        protected override void OnDeleted()
        {
        }
    }
}
