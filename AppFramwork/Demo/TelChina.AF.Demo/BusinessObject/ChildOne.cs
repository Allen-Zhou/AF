using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.AF.Demo
{
    public class ChildOne:Parent
    {
        //public override Guid ID
        //{
        //    get; 
        //    set;
        //}

        //public override string FirstName
        //{
        //    get; 
        //    set; 
        //}

        //public override string LastName
        //{
        //    get;
        //    set; 
        //}

        public virtual String Email
        {
            get;
            set;
        }
    }
}
