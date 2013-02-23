using System;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class Product : EntityBase
    {

        #region Declarations
        
        private string _code = string.Empty;
        private string _name = String.Empty;
        private int _size = default(Int32);

        #endregion

        #region Properties

        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public virtual int Size
        {
            set { _size = value; }
            get { return _size; }
        }

        private Category _category;

        /// <summary>
        /// 该产品的部门
        /// </summary>
        public virtual Category Category
        {
            get { return _category; }
            set { _category = value; }
        }


        #endregion 
    }

}