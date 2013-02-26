using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class Category : EntityBase
    {
        public override string EntityComponent
        {
            get;
            set;
        }

        #region Declarations

        private string _description = string.Empty;
        private string _name = String.Empty;
        private int _size = default(int);


        #endregion

        #region Properties

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public virtual int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        private Product _product;

        /// <summary>
        /// 产品
        /// </summary>
        public virtual Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        #endregion
    }
}
